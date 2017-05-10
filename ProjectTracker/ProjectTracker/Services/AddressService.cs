using Newtonsoft.Json;
using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Services
{

    public class Address
    {
        public string Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }

        public string FullStreetAddress
        {
            get
            {
                string output = this.Address1;

                if (!String.IsNullOrEmpty(this.Address2))
                    output += ", " + this.Address2;

                if (!String.IsNullOrEmpty(this.Address3))
                    output += ", " + this.Address3;

                return output;
            }
        }
        public string CityProvincePostalCode
        {
            get
            {
                return City + ", " + Province + " " + PostalCode;
            }
        }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Status { get; set; }
        public string County { get; set; }
        public string Country { get; set; }

        public string AddressHash { get; set; }

        public Address()
        {

        }
    }

    public static class AddressService
    {
        public static async Task<APIAddressResource> ReturnValidatedAddress(Address address)
        {
            APIAddressResource output = null;

            string queryString = "";
            if (!String.IsNullOrEmpty(address.Address1))
            {
                queryString += address.Address1.Replace(" ", "%20") + "%20";
            }
            if (!String.IsNullOrEmpty(address.Address2))
            {
                queryString += address.Address2.Replace(" ", "%20") + "%20";
            }
            if (!String.IsNullOrEmpty(address.Address3))
            {
                queryString += address.Address3.Replace(" ", "%20") + "%20";
            }
            if (!String.IsNullOrEmpty(address.City))
            {
                queryString += address.City + "%20";
            }
            if (!String.IsNullOrEmpty(address.Province))
            {
                queryString += address.Province + "%20";
            }
            if (!String.IsNullOrEmpty(address.PostalCode))
            {
                queryString += address.PostalCode + "%20";
            }

            string json = await Utilities.WebServiceRequest("http://dev.virtualearth.net/REST/v1/Locations?q=" + queryString + "&o=json&key=" + GlobalConfig.MapsAPIKey);
            LocationRootobject rootObject = JsonConvert.DeserializeObject<LocationRootobject>(json);

            if (rootObject.resourceSets.Count() > 0)
            {
                var resourceSet = rootObject.resourceSets[0];
                if (resourceSet.resources.Count() == 0)
                {
                    throw new Exception("No matches");
                }
                if (resourceSet.resources.Count() == 1)
                {
                    var resource = resourceSet.resources[0];
                    if (resource.address != null && resource.entityType.ToLower() == "address")
                    {
                        output = resource;
                    }
                    else
                    {
                        throw new Exception("Invalid address");
                    }
                }
                else
                {
                    throw new Exception("Multiple matches");
                }
            }

            return output;
        }
    }
}
