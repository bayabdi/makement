using Api.Models.Organization;
using Api.Models.Track;
using BLL.Services.Interfaces;
using Common.Enum;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class TrackingService : Service, ITrackingService
    {
        public TrackingService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void SaveActivity(ActivityModel activity)
        {
            var data = activity.File.Split("\r\n");
            var model = new UserActivity();
            model.Date = activity.Date;
            model.UserId = activity.UserId;
            model.ActivityTime = TimeSpan.FromSeconds(int.Parse(DecryptString(data[0])));
            model.AbsenceTime = TimeSpan.FromSeconds(int.Parse(DecryptString(data[1])));

            UnitOfWork.UserActivities.Save(model);
            UnitOfWork.Commit();
        }

        public void SaveAppInfo(AppInfoModel info)
        {
            var data = info.File.Split("\r\n");
      
            for (int i = 0; i < data.Length; i++)
            {
                string row = DecryptString(data[i]);

                if (row == "")
                {
                    continue;
                }
                int second = int.Parse(row.Split('@').Last());
                string name = row.Substring(0, row.LastIndexOf('@'));

                var model = new AppInfo
                {
                    Time = TimeSpan.FromSeconds(second),
                    Info = (name != "")? name : "Desktop",
                    Date = info.Date,
                    UserId = info.UserId,
                };
                
                UnitOfWork.AppInfo.Save(model);
                UnitOfWork.Commit();

                /*if (text[i] == '\r')
                {
                    i += 1;
                    result = DecryptString(str);
                    int dog = 0;
                    for (int j = result.Length - 1; j >= 0; j--)
                    {
                        if (result[j] == '@')
                        {
                            dog = j;
                            break;
                        }
                    }
                    var x = result.Substring(dog + 1, result.Length - dog - 1);
                    
                    str = "";
                }
                else
                {
                    str += text[i];
                }*/
            }
        }

        public static string ReadFile(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            return result.ToString();
        }

        public static string DecryptString(string text)
        {
            string key = "c12ca5898a4e4134bbce2ea3316a1916";
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public void SaveLocation(LocationModel model)
        {
            UnitOfWork.Locations.Add(new Location
            {
                Accuracy = model.Accuracy,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Address = model.Address,
                Altitude = model.Altitude,
                Speed = model.Speed,
                Time = model.Date,
                UserId = model.UserId
            });
            UnitOfWork.Commit();
        }

        public LocationViewModel GetLocation(string userId)
        {
            var list = UnitOfWork.Locations.GetAll().Result;
            var location = list.First(x => x.UserId == userId && x.Time == list.Max(x => x.Time));

            return new LocationViewModel
            {
                Id = location.Id,
                Accuracy = location.Accuracy,
                Address = location.Address,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Altitude = location.Altitude,
                Speed = location.Speed,
                Time = location.Time,
                UserId = userId
            };
        }

        public IEnumerable<AppInfoViewModel> GetAppInfo(string userId)
        {
           var list = UnitOfWork.AppInfo.GetAll().Result
                .Where(x => x.UserId == userId && x.Date.Date == DateTime.Now.Date)
                .Select(x => new AppInfoViewModel { 
                    Id = x.Id,
                    Info = x.Info,
                    Date = x.Date.Date,
                    Time = x.Time,
                    UserId = userId
                });

            return list;
        }

        public ActivityViewModel GetActivity(string userId)
        {
            var activity = UnitOfWork.UserActivities.GetAll()
                .Result.First(x => x.Date.Date == DateTime.Now.Date && x.UserId == userId);

            return new ActivityViewModel
            {
                Id = activity.Id,
                AbsenceTime = activity.AbsenceTime,
                ActivityTime = activity.ActivityTime,
                Date = activity.Date.Date,
                UserId = userId
            };
        }

        public IEnumerable<LocationViewModel> FilterLocation(LocationFilterModel model)
        {
            var locations = UnitOfWork.Locations.GetAll().Result.Where(x => x.UserId == model.UserId);
            if (model.PastHour == LocationFilterEnum.OneHour)
            {
                locations = locations.Where(x => (DateTime.Now - x.Time).TotalHours <= 1);
            }
            else if (model.PastHour == LocationFilterEnum.TwoHour)
            {
                locations = locations.Where(x => (DateTime.Now - x.Time).TotalHours <= 2);
            }
            else if (model.PastHour == LocationFilterEnum.Today)
            {
                locations = locations.Where(x => (DateTime.Now.Day == x.Time.Day));
            }
            else if (model.PastHour == LocationFilterEnum.Yesterday)
            {
                locations = locations.Where(x => DateTime.Compare(x.Time.Date, DateTime.Now.AddDays(-1).Date) == 0);
            }
            else if (model.PastHour == LocationFilterEnum.Week)
            {
                locations = locations.Where(x => DateTime.Now.AddDays(-7) <= x.Time);
            }
            else if (model.PastHour == LocationFilterEnum.Month)
            {
                locations = locations.Where(x => DateTime.Now.AddDays(-30) <= x.Time);
            }

            return mapper.Map<IEnumerable<Location>, IEnumerable<LocationViewModel>>(locations);
        }

        public void SaveAppInfoMobile(SaveAppInfoMobileModel model)
        {
            var info = mapper.Map<SaveAppInfoMobileModel, AppInfoMobile>(model);
            info.User = UnitOfWork.Users.Get(model.UserId).Result;
            var names = info.PackageName.Split('.');
            
            info.PackageName = names[names.Length - 1];

            var apps = UnitOfWork.AppInfoMobile.GetAll().Result.FirstOrDefault(x =>
                            x.UserId == info.UserId && x.PackageName == info.PackageName 
                            && x.FirstTimeStamp.Date == info.FirstTimeStamp.Date);
            if (apps == null)
            {
                UnitOfWork.AppInfoMobile.Add(apps);
            }
            else
            {
                model.TotalTime = info.TotalTime;
                UnitOfWork.AppInfoMobile.Update(apps);
            }

            UnitOfWork.Commit();
        }

        public AppInfoPagViewModel AppFilter(AppFilterModel model)
        {
            var apps = UnitOfWork.AppInfo.GetAll().Result.Where(x => x.UserId == model.UserId);
            AppInfoPagViewModel appInfo = new AppInfoPagViewModel();

            if (model.Date == TrackFilterEnum.Today)
            {
                apps = apps.Where(x => (DateTime.Now.Day == x.Date.Day));
            }
            else if (model.Date == TrackFilterEnum.Yesterday)
            {
                apps = apps.Where(x => (DateTime.Today.AddDays(-1) <= x.Date && x.Date < DateTime.Today));
            }

            appInfo.TotalApps = apps.Count();
            apps = apps.Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize);
            appInfo.Apps = mapper.Map<IEnumerable<AppInfo>, IEnumerable<AppInfoViewModel>>(apps);

            return appInfo;
        }

        public IEnumerable<ActivityViewModel> ActivityFilter(ActivityFilterModel model)
        {
            var activities = UnitOfWork.UserActivities.GetAll().Result.Where(x => x.UserId == model.UserId);
            if (model.Date == TrackFilterEnum.Today)
            {
                activities = activities.Where(x => (DateTime.Now.Day == x.Date.Day));
            }
            else if (model.Date == TrackFilterEnum.Yesterday)
            {
                activities = activities.Where(x => (DateTime.Today.AddDays(-1) <= x.Date && x.Date < DateTime.Today));
            }

            return mapper.Map<IEnumerable<UserActivity>, IEnumerable<ActivityViewModel>>(activities);
        }

        public AppInfoPagViewModel AppMobileFilter(AppFilterModel model)
        {
            var apps = UnitOfWork.AppInfoMobile.GetAll().Result.Where(x => x.UserId == model.UserId);
            
            AppInfoPagViewModel appInfo = new AppInfoPagViewModel();

            if (model.Date == TrackFilterEnum.Today)
            {
                apps = apps.Where(x => (DateTime.Now.Day == Convert.ToDateTime(x.FirstTimeStamp).Day));
            }
            else if (model.Date == TrackFilterEnum.Yesterday)
            {
                apps = apps.Where(x => (DateTime.Today.AddDays(-1) <= Convert.ToDateTime(x.FirstTimeStamp) && Convert.ToDateTime(x.FirstTimeStamp) < DateTime.Today));
            }
            
            appInfo.TotalApps = UnitOfWork.AppInfoMobile.GetAll().Result.Where(x => x.UserId == model.UserId).Count();
            apps = apps.Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize);
            appInfo.Apps = mapper.Map<IEnumerable<AppInfoMobile>, IEnumerable<AppInfoViewModel>>(apps);            
            return appInfo;
        }

        public void SaveScreenShot(SaveScreenShotModel model)
        {
            var screenShot = new ScreenShot();

            screenShot.Id = Guid.NewGuid().ToString();
            screenShot.UserId = model.UserId;
            screenShot.User = UnitOfWork.Users.Get(model.UserId).Result;
            screenShot.Type = "image/jpeg";
            screenShot.Img = Convert.FromBase64String(model.File);
            screenShot.Time = model.DateTime;

            UnitOfWork.ScreenShot.Add(screenShot).Wait();
            UnitOfWork.Commit();
        }

        public ScreenShotPagViewModel ScreenShotsFilter(ScreenShotsFilterModel model)
        {
            var list = UnitOfWork.ScreenShot.GetAll().Result
                .Where(x => x.UserId == model.UserId);

            if (model.Date == TrackFilterEnum.Today)
            {
                list = list.Where(x => (DateTime.Now.Day == Convert.ToDateTime(x.Time).Day));
            }
            else if (model.Date == TrackFilterEnum.Yesterday)
            {
                list = list.Where(x => (DateTime.Today.AddDays(-1) <= Convert.ToDateTime(x.Time) && Convert.ToDateTime(x.Time) < DateTime.Today));
            }

            ScreenShotPagViewModel screenShot = new ScreenShotPagViewModel();
            screenShot.TotalScreenShots = list.Count();
            list = list
                .Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize);
            screenShot.ScreenShot = mapper.Map<IEnumerable<ScreenShot>, IEnumerable<ScreenShotViewModel>>(list);
            return screenShot;
        }
    }
}
