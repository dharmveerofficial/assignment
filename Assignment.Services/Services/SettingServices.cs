using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.Data;
using Assignment.Data.Models;
using Assignment.Identity.ViewModels;
using Assignment.Identity.IServices;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Assignment.Services.Services
{
    public class SettingServices : ISettingServices
    {
        List<SettingViewModel> list = new List<SettingViewModel>();
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SettingServices(AppDbContext dbcontext, IMapper mapper)
        {
            _dbContext = dbcontext;
            _mapper = mapper;
        }
        public object getSetting(dynamic obj)
        {
            int total = 0;
            int filter = 0;
            var draw = obj["draw"];
            var sortColumn = obj["sortColumn"];
            var sortColumnDirection = obj["sortColumnDirection"];
            var searchValue = obj["searchValue"];
            int skip = Convert.ToInt32(obj["skip"] ?? "0");
            int take = Convert.ToInt32(obj["take"] ?? "0");
            try
            {
                var data = _dbContext.Set<Setting>().Where(x => x.IsDeleted == false).AsEnumerable();
                total = data.Count();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(x => x.Value.ToLower().Contains(searchValue.ToLower()) || x.Value2.ToLower().Contains(searchValue.ToLower()) || x.Key.ToLower().Contains(searchValue.ToLower()));
                }
                filter = data.Count();

                var propertyInfo = typeof(Setting).GetProperty(sortColumn);

                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) data = sortColumnDirection == "asc" ? data.OrderBy(o => propertyInfo.GetValue(o, null)) : data.OrderByDescending(o => propertyInfo.GetValue(o, null));
                var settings = data.Skip(skip).Take(take).ToList();
                list = _mapper.Map<List<SettingViewModel>>(settings);
                var returnObj = new
                {
                    draw = draw,
                    recordsTotal = total,
                    recordsFiltered = filter,
                    data = list
                };

                return returnObj;
            }
            catch (Exception e)
            {
                return new
                {
                    draw = draw,
                    recordsTotal = total,
                    recordsFiltered = filter,
                    msg = "Something went wrong"
                };
            }
        }



        public string AddSetting(SettingViewModel payload)
        {
            try
            {
                var add = new Setting()
                {
                    Key = payload.Key,
                    Value = payload.Value,
                    Value2 = payload.Value2,
                    Description = payload.Description,
                    Created = payload.Created,
                    LastModified = payload.LastModified,
                    IsDeleted = false,
                };
                _dbContext.Setting.Add(add);
                _dbContext.SaveChanges();

                if (add.Id > 0)
                {
                    return "success";
                }
                else
                {
                    return "fail";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string UpdateSetting(SettingViewModel payload)
        {
            try
            {
                var previos = _dbContext.Setting.AsNoTracking().Where(g => g.Id == payload.Id).FirstOrDefault();

                if (previos != null)
                {
                    var update = new Setting()
                    {
                        Id = payload.Id,
                        Key = payload.Key,
                        Value = payload.Value,
                        Value2 = payload.Value2,
                        Description = payload.Description,
                        Created = previos.Created,
                        LastModified = DateTime.Now,
                        IsDeleted = previos.IsDeleted,
                    };
                    _dbContext.Setting.Update(update);
                    _dbContext.SaveChanges();
                    return "success";
                }
                else
                {
                    return "fail";
                }
            }

            catch (Exception e)
            {
                return e.Message;

            }
        }

        public string DeleteSetting(Int64 id)
        {
            try
            {
                var data = _dbContext.Setting.FirstOrDefault(x => x.Id == id);
                data.IsDeleted = true;
                _dbContext.SaveChanges();
                return "Deleted";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


    }
}
