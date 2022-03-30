using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Company> GetAllCompanies()
        {
            return new List<Company>()
            {
                new Company(){ Id=1, Name="AOne"},
                new Company(){ Id=2, Name="BTwo"},
                new Company(){ Id=3, Name="CThree"},
                new Company(){ Id=4, Name="DFour"},
                new Company(){ Id=5, Name="EFive"}
            };


            //return new Company[]  {
            //    new Company(){ Id=1, Name="AOne"},
            //    new Company(){ Id=2, Name="BTwo"},
            //    new Company(){ Id=3, Name="CThree"},
            //    new Company(){ Id=4, Name="DFour"},
            //    new Company(){ Id=5, Name="EFive"}
            //};

        }
    }


    

  



}