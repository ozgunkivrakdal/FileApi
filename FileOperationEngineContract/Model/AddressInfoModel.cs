using System;
using System.Collections.Generic;
using System.Text;

namespace FileOperationEngineContract.Model
{
    public class AddressInfoModel
    {
        public string cityName { get; set; }
        public string cityCode { get; set; }
        public string districtName { get; set; }
        public string zipCode { get; set; }

        public AddressInfoModel() { }
        public AddressInfoModel(string name, string code, string district, string zip)
        {
            this.cityName = name;
            this.cityCode = code;
            this.districtName = district;
            this.zipCode = zip;

        }
    }
}
