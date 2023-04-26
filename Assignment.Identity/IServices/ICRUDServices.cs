using Assignment.Identity.ViewModels;
using System;
using System.Collections.Generic;

namespace Assignment.Identity.IServices
{
    public interface ICRUDServices
    {
        object getSetting(dynamic obj);
        string AddSetting(SettingViewModel payload);
        string UpdateSetting(SettingViewModel payload);
        string DeleteSetting(Int64 id);

    }
}
