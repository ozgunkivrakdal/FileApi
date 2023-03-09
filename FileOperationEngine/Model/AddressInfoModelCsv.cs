using System;
using System.Collections.Generic;
using System.Text;

namespace FileOperationEngine.Model
{
    public class AddressInfoModelCsv
    {
        public string cityName { get; set; }
        public string cityCode { get; set; }
        public string districtName { get; set; }
        public string zipCode { get; set; }

        public static AddressInfoModelCsv FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            AddressInfoModelCsv address = new AddressInfoModelCsv();
            address.cityName = values[0];
            address.cityCode = values[1];
            address.districtName = values[2];
            address.zipCode = values[3];
            return address;
        }
    }
}
