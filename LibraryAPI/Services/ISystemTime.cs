using System;

namespace LibraryAPI.Services
{
    public interface ISystemTime
    {
        DateTime GetCurrent();
    }
}