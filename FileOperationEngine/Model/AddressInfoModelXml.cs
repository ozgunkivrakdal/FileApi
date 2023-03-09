using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileOperationEngine.Model
{
    [XmlRoot(ElementName = "AddressInfo")]
    public class AddressInfoModelXml
    {
        [XmlElement(ElementName = "City")]
        public List<City> cities { get; set; }

        public AddressInfoModelXml()
        {
            cities = new List<City>();
        }

        public class City
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("code")]
            public string Code { get; set; }
            [XmlElement(ElementName = "District")]
            public List<District> districts { get; set; }

            public City()
            {
                districts = new List<District>();
            }

        }


        public class District
        {
            [XmlAttribute("name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "Zip")]
            public List<Zip> Zipcodes { get; set; }
            public District()
            {
                Zipcodes = new List<Zip>();
            }

        }

        public class Zip
        {
            [XmlAttribute("code")]
            public string Code { get; set; }

        }
       
    }
}
