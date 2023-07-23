﻿using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Web.WebPages.Html;

namespace WebApplication1.Models
{
    public class Class1
    {
    }


    public static class globalclass
    {
        public static string username11 = "Wilson";
    }



    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated from a template.
    //
    //     Manual changes to this file may cause unexpected behavior in your application.
    //     Manual changes to this file will be overwritten if the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

  
        

        public partial class yUser
        {
            public int userid { get; set; }



            [Required(ErrorMessage = "Must fill up username")]
            public string username { get; set; }


            [Required(ErrorMessage = "Must fill up email")]
            public string email { get; set; }


            [Required(ErrorMessage = "Must fill up password")]
            [DataType(DataType.Password)]
            public string password { get; set; }


            public string usertype { get; set; }


            public string nomatchingUsers { get; set; }
        }

        public partial class yNewUser
        {

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int userid { get; set; }



            [Required(ErrorMessage = "Must fill up username")]
            public string username { get; set; }


            [Required(ErrorMessage = "M ust fill up email")]
            [DataType(DataType.EmailAddress, ErrorMessage = " must be valid email")]         
            public string email1 { get; set; }



            [Required(ErrorMessage = "must fill up email")]
        [DataType(DataType.EmailAddress, ErrorMessage = " must be valid email")]
        [Compare("email1", ErrorMessage = "email must match")]
            public string confirmemail { get; set; }


            [Required(ErrorMessage = "must fill up password")]
            [DataType(DataType.Password)]
            public string password { get; set; }



            [DataType(DataType.Password)]
            [Compare("password", ErrorMessage = "password must match")]
            public string confirmpassword { get; set; }


         [Required(ErrorMessage ="must fill up contact information")]
         [DataType(DataType.PhoneNumber , ErrorMessage =" must be a valid phone number")]
         public string contactnumber { get; set; }

        [Required(ErrorMessage ="must fill up address")]
         public string city { get; set; }
         public string vaccinated { get; set; }

        [Required(ErrorMessage = "must fill up first name")]
        public string fname { get; set; }
        [Required(ErrorMessage = "must fill up last name")]
        public string lname { get; set; }

        public string usertype { get; set; }


        public string userconfirmation { get; set; }

       


    }




    public partial class ycheckSchedule
    {
        public int numberofpassengers { get; set; }
        public string contactnumber { get; set; }
        public bool termsandconditions { get; set; }

        [Required]
        public int? kilo {get;set;}


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string datecreated { get; set; }
        public Nullable<int> user_id { get; set; }
        public string status { get; set; }
        public Nullable<int> schedule_id { get; set; }




    }


        public partial class yGroup1 : Group
        {

            public string isHidden1 { get; set; } = "normal";
            public int info_id1 { get; set; }
            public Nullable<int> groupnumber1 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string firstname1 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string surname1 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string middle1 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            [Range(3, 100, ErrorMessage ="must be between age 3 to 100 ")]
            public Nullable<int> age1 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string passengertype1 { get; set; }
            public bool isEnsured1 { get; set; }
            public bool isVaxxed1 { get; set; }

            public string isHidden2 { get; set; } = "normal";
            public int info_id2 { get; set; }
            public Nullable<int> groupnumber2 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string firstname2 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string surname2 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string middle2 { get; set; }
            [Required(ErrorMessage = "this field is required")]
        [Range(3, 100, ErrorMessage = "must be between age 3 to 100 ")]
        public Nullable<int> age2 { get; set; }
            public string passengertype2 { get; set; }
            public bool isEnsured2 { get; set; }
        public bool isVaxxed2 { get; set; }



        public string isHidden3 { get; set; } = "normal";
            public int info_id3 { get; set; }
            public Nullable<int> groupnumber3 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string firstname3 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string surname3 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string middle3 { get; set; }
            [Required(ErrorMessage = "this field is required")]
        [Range(3, 100, ErrorMessage = "must be between age 3 to 100 ")]
        public Nullable<int> age3 { get; set; }
            public string passengertype3 { get; set; }
            public bool isEnsured3 { get; set; }
        public bool isVaxxed3 { get; set; }



        public string isHidden4 { get; set; } = "normal";
            public int info_id4 { get; set; }
            public Nullable<int> groupnumber4 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string firstname4 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string surname4 { get; set; }
            [Required(ErrorMessage = "this field is required")]
            public string middle4 { get; set; }
            [Required(ErrorMessage = "this field is required")]
        [Range(3, 100, ErrorMessage = "must be between age 3 to 100 ")]
        public Nullable<int> age4 { get; set; }
            public string passengertype4 { get; set; }
            public bool isEnsured4 { get; set; }
        public bool isVaxxed4 { get; set; }


    }


        public partial class yadditionaldetails
        {
            public string faretype { get; set; }
            public string methodofpayment { get; set; }
            public string clientEmail { get; set; }
            [Required(ErrorMessage ="please input contact number")]
            public string clientContact { get; set; }




              public bool terms { get; set; }
       
            
            

        }




    public partial class ySchedule
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int schedule_id { get; set; }


        [Required]
        public string transport_name { get; set; }


        [Required]
        public string departuretime { get; set; }

        [Required]    
        public string arrivaltime { get; set; }

        [Required]
        public Nullable<int> route_id { get; set; }
        
   
    }


    public partial class yCargobooking
    {
  
    }


    public partial class yeditprofile
    {

        [Required(ErrorMessage = "email must not be blank")]
        [DataType(DataType.EmailAddress, ErrorMessage = "must be a valid email")]
        public  string email { get; set; }
        [Required(ErrorMessage = "password must not be blank")]
        [DataType(DataType.Password)]
        public  string password { get; set; }

        [Required(ErrorMessage = "password must not be blank")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "password must match")]
        public  string confirmpassword { get; set; }


        [Required(ErrorMessage = "must fill up contact information")]
        [DataType(DataType.PhoneNumber, ErrorMessage = " must be a valid phone number")]
        public string contactnumber { get; set; }

        [Required(ErrorMessage = "must fill up address")]
        public string city { get; set; }
        public string vaccinated { get; set; }

        [Required(ErrorMessage = "must fill up first name")]
        public string fname { get; set; }
        [Required(ErrorMessage = "must fill up last name")]
        public string lname { get; set; }


    }






    public partial class yRoutepage
    {

        public int routeId { get; set; }

        [Required]
        public string terminalfrom { get; set; }


        [Required]     
        public string terminalto { get; set; }
        [Required]
        public Nullable<double> price1 { get; set; }
        [Required]
        public Nullable<double> price2 { get; set; }
        [Required]
        public Nullable<double> price3 { get; set; }
        [Required]
        public Nullable<double> price4 { get; set; }
        [Required]
        public Nullable<double> pricecargo { get; set; }
        [Required]
        public string routemode { get; set; }


        [Required]
        public string terminalname { get; set; }
        [Required]
        public string terminallocation { get; set; }



        [Required]
        public string transportname { get; set; }
        [Required]
        public string trasnporttype { get; set; }
        [Required]
        public string transportstatus { get; set; }

        public string searchtxt { get; set; }


    }


}
