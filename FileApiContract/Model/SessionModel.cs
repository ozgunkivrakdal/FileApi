using System;
using System.Collections.Generic;
using System.Text;

namespace FileApiContract.Model
{
    public class SessionModel
    {
        public int userId { get; set; }
        public int timezoneId { get; set; }

        // all necessary item can be stored into session 
    }
}
