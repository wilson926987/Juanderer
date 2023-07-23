using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public partial class browseFromandTo
    {
        public SelectList locationTo { get; set; }
        public SelectList locationFrom { get; set; }
    }


    public static class tempclass
    {

        public static int? indextemp { get; set; }
        public static int? numberpassengertemp { get; set; }
        public static string modalstate { get; set; }
        public static string mustaccept { get; set; }

    }

    public class fullpricing
    {
        public double? priceperhead { get; set; }
        public  double? grossprice { get; set; }
        public double? seniordiscount  { get; set; }
        public  double? notaxprice { get; set; }
        public  double? taxamount { get; set; }
        public double? finalprice { get; set; }


    }



    public static class cargorecord
    {



    }

    public static class fullrecord
    {

        public static string datecreated { get; set; }
        public static int? numberofpassengers { get; set; } = 1;
        public static int? scheduleindex { get; set; }
        public static double? pricetouristclass { get; set; }
        public static double? pricebusinessclass { get; set; }
        public static double? pricebusinessclass2 { get; set; }
        public static double? priceregular { get; set; }
        public static string clientEmail { get; set; }
        public static string clientContact { get; set; }
        public static double seniourdiscountmultiplier { get; set; }
        public static int numberofseniours { get; set; }
        public static double totalseniordiscount{get;set;}
        public static yGroup1 thenewgroup1;
        public static string payment_option { get; set; }
        public static double? pricecargo { get; set; }
        public static double? inputkilo { get; set; }
       
    }



    public partial class cargolist
    {

       public string departure { get; set; }
        public string arrival { get; set; }
        public string locationfrom { get; set; }
        public string locationto { get; set; }
        public double? itemweight { get; set; }
        public string status { get; set; }
        public int? cargo_id { get; set; }
    }


 



    public partial class schedulelist
    {
       public int? sched_id { get; set; }
        public string departuretime { get; set; }
        public string arrivaltime { get; set; }
        public string locationfrom { get; set; }
        public string locationto { get; set; }
        public string mode { get; set; }
        public float priceregular { get; set; }
        public float pricetourist { get; set; }
        public float pricebusiness { get; set; }
        public float pricebusiness2 { get; set; }

        public string priceregular2 { get; set; }

        public string terminalfrom { get; set; }
        public string terminalto { get; set; }
        public string transportname { get; set; }


        public string paymenttype { get; set; }
        public int? numberofpassenger { get; set; }
        public int? book_id { get; set; }
        public string sched_status { get; set; }

       
    }











    public class HomeController : Controller
    {


      

        public ActionResult Index()
        {

            ViewBag.Currentuser = CurrentUser.username;

          
            Database1Entities1 db = new Database1Entities1();



            //  ViewBag.From1 = db.Routes.Select(o => o.locationfrom).Distinct().ToList();
            ViewBag.From1 = db.Routes.Join(db.Schedules.Where(p => p.sched_status =="open"), r => r.route_id, s => s.route_id, (route, sched) => new { route, sched }).Select(o => o.route.locationfrom).Distinct().ToList();


            // ViewBag.To1 = db.Routes.Select(o => o.locationto).Distinct().ToList();
            ViewBag.To1 = db.Routes.Join(db.Schedules.Where(p => p.sched_status == "open"), r => r.route_id, s => s.route_id, (route, sched) => new { route, sched }).Select(o => o.route.locationto).Distinct().ToList();

            List<string> transportypes = new List<string>() { "all", "land", "air", "sea" };

            ViewBag.Mode1 = transportypes;


            if(tempclass.modalstate!= null)
            {
                ViewBag.modalstate = tempclass.modalstate;
                tempclass.modalstate = null;
            }


            return View();
        }

        public ActionResult Login()
        {

            switch (CurrentUser.redirect11())
            {
                case "admin": return RedirectToAction("Dashboard", "Admin", null);
                case "client": return RedirectToAction("Index", "Home", null);
                default: return View();

            }
        
        }

        [HttpPost]
        public ActionResult trylogin(yUser temp1)
        {

            using(Database1Entities1 db = new Database1Entities1())
            {
                var temp = db.Users.FirstOrDefault(x => x.username == temp1.username && x.password == temp1.password);

                if (temp == null)
                {
                    temp1.nomatchingUsers = "username and password do not match";
                    return View("Login", temp1);
                }
                else
                {
                    CurrentUser.userId = temp.userid;
                    CurrentUser.usertype = temp.usertype.Trim();
                    CurrentUser.username = temp.username.Trim();

                    fullrecord.clientEmail = temp.email;
                    fullrecord.clientContact = temp.contactnumber;
                    return RedirectToAction("PageRedirect", "Admin", null);
                }


            }         
        }



        public ActionResult logout()
        {
            CurrentUser.userId = null;
            CurrentUser.usertype = "";
            CurrentUser.username = null;

            return RedirectToAction("PageRedirect", "Admin", null);
        }


        public ActionResult Register()
        {
            CurrentUser.userId = null;
            CurrentUser.usertype = "";
            CurrentUser.username = null;
            return View();
        }



        [HttpPost]
        public ActionResult tryregister(yNewUser temp1)
        {
            Database1Entities1 db = new Database1Entities1();

            var userconfirm = db.Users.FirstOrDefault(m => m.username == temp1.username);
            if(userconfirm != null)
            {
                temp1.userconfirmation = "username already exists";
                return View("Register", temp1);
            }






           
                User newuser1 = new User();
                newuser1.username = temp1.username;
                newuser1.password = temp1.password;
                newuser1.email = temp1.email1;
                newuser1.usertype = "client";
                newuser1.contactnumber = temp1.contactnumber;
                newuser1.city = temp1.city;
                newuser1.fname = temp1.fname;
                newuser1.lname = temp1.lname;



                db.Users.Add(newuser1);
                db.SaveChanges();
            


            return Redirect("Login");
        }








        [HttpPost]
        public ActionResult browseschedule(Route temp1)
        {
            ViewBag.Currentuser = CurrentUser.username;

            Database1Entities1 db = new Database1Entities1();

            ViewBag.From1 = db.Routes.Join(db.Schedules.Where(p => p.sched_status == "open"), r => r.route_id, s => s.route_id, (route, sched) => new { route, sched }).Select(o => o.route.locationfrom).Distinct().ToList();


            // ViewBag.To1 = db.Routes.Select(o => o.locationto).Distinct().ToList();
            ViewBag.To1 = db.Routes.Join(db.Schedules.Where(p => p.sched_status == "open"), r => r.route_id, s => s.route_id, (route, sched) => new { route, sched }).Select(o => o.route.locationto).Distinct().ToList();

            List<string> transportypes = new List<string>(){ "all","land", "air", "sea" };

            ViewBag.Mode1 = transportypes;

            string locationto1 = temp1.locationto;
            string locationfrom1 = temp1.locationfrom;
            List<Schedule> schedule1 = db.Schedules.ToList();

            if (temp1.mode == null)
            {
                temp1.mode = "all";
            }

           

            if(temp1.mode == "all")
            {
                if(temp1.locationfrom != null & temp1.locationto == null)
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status=="open").Join(db.Routes.Where(m => m.locationfrom == temp1.locationfrom), r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.schedule_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
              
                    ViewBag.Schedule1 = temp2;
                }else if (temp1.locationfrom == null && temp1.locationto != null)
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status == "open").Join(db.Routes.Where(m => m.locationto == temp1.locationto), r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.schedule_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
          
                    ViewBag.Schedule1 = temp2;
                }else if(temp1.locationfrom != null && temp1.locationto != null)
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status == "open").Join(db.Routes.Where(m => m.locationfrom == temp1.locationfrom && m.locationto == temp1.locationto), r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.schedule_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
                
                    ViewBag.Schedule1 = temp2;
                }
                else
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status == "open").Join(db.Routes, r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.schedule_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
                   
                    ViewBag.Schedule1 = temp2;
                }
            }
            else
            {
                if (temp1.locationfrom != null & temp1.locationto == null)
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status == "open").Join(db.Routes.Where(m => m.locationfrom == temp1.locationfrom && m.mode == temp1.mode), r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.schedule_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
                
                    ViewBag.Schedule1 = temp2;
                }
                else if (temp1.locationfrom == null && temp1.locationto != null)
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status == "open").Join(db.Routes.Where(m => m.locationto == temp1.locationto && m.mode == temp1.mode), r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.route_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
                   
                    ViewBag.Schedule1 = temp2;
                }
                else if (temp1.locationfrom != null && temp1.locationto != null)
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status == "open").Join(db.Routes.Where(m => m.locationfrom == temp1.locationfrom && m.locationto == temp1.locationto && m.mode == temp1.mode), r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.schedule_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
              
                    ViewBag.Schedule1 = temp2;
                }
                else
                {
                    var temp2 = db.Schedules.Where(p => p.sched_status == "open").Join(db.Routes.Where(m => m.mode == temp1.mode), r => r.route_id, s => s.route_id, (schedules, routes) => new schedulelist()
                    {
                        sched_id = (int)schedules.schedule_id,
                        departuretime = schedules.departuretime,
                        arrivaltime = schedules.arrivaltime,
                        locationfrom = routes.locationfrom,
                        locationto = routes.locationto,
                        mode = routes.mode
                    }); ;
                
                    ViewBag.Schedule1 = temp2;
                }

            }








            return View();
            
        }



        public ActionResult checkschedule(int? index)
        {
            if(CurrentUser.userId== null)
            {
                return Redirect("Login");
            }


            ViewBag.Currentuser = CurrentUser.username;

            if (index== null)
            {
                index = fullrecord.scheduleindex;
            }

            if(fullrecord.scheduleindex == null && index == null)
            {
                return Redirect("Index");
            }


            if(tempclass.numberpassengertemp != null)
            {
                ViewBag.numberpassengertemp = tempclass.numberpassengertemp;
        
            }
            tempclass.numberpassengertemp = null;

            tempclass.indextemp = index;
            fullrecord.scheduleindex = index;
            

            List<int> transportypes = new List<int>() {1,2,3,4 };

            ViewBag.temp8 = transportypes;



            Database1Entities1 db = new Database1Entities1();
            
               var tempsched = db.Schedules.FirstOrDefault(m => m.schedule_id == index);
            
                if (tempsched != null)
                {    
                
                Route temproute = db.Routes.FirstOrDefault(n => n.route_id == tempsched.route_id);

                ViewBag.locationfrom = temproute.locationfrom;
                ViewBag.locationto = temproute.locationto;
                ViewBag.terminalfrom = temproute.terminalfrom;
                ViewBag.terminalto = temproute.terminalto;

                ViewBag.departuretime = tempsched.departuretime;
                ViewBag.arrivaltime = tempsched.arrivaltime;
                ViewBag.transportname = tempsched.transport_name;
                ViewBag.priceregular = temproute.updatedpriceregular_;
                ViewBag.pricetourist = temproute.updatedpricetourist;
                ViewBag.pricebusiness = temproute.updatedpricebusiness;
                ViewBag.pricebusiness2 = temproute.updatedpricebusiness2;
                ViewBag.pricecargo = temproute.updatedpricecargo;
                ViewBag.mode1 = temproute.mode;


                fullrecord.priceregular = temproute.updatedpriceregular_;
                fullrecord.pricetouristclass = temproute.updatedpricetourist;
                fullrecord.pricebusinessclass = temproute.updatedpricebusiness;
                fullrecord.pricebusinessclass2 = temproute.updatedpricebusiness2;
                fullrecord.pricecargo = temproute.updatedpricecargo;


                return View();       
                }

            MessageBox.Show("may problema dito");
            return Redirect("Index");
        }




        [HttpPost]
        public ActionResult confirmvaxx(ycheckSchedule checkschedule)
        {
            tempclass.numberpassengertemp = checkschedule.numberofpassengers;
            return Redirect("checkschedule");


        }

        


        [HttpPost]
        public ActionResult InputPassengerInfo(ycheckSchedule checkSchedule)
        {

            if (CurrentUser.userId == null)
            {
                return Redirect("Login");
            }


            ViewBag.Currentuser = CurrentUser.username; ///kailangan to
            if (checkSchedule.numberofpassengers == 0)
            {
                checkSchedule.numberofpassengers = 1;
            }


            ViewBag.number1 = checkSchedule.numberofpassengers;
            fullrecord.numberofpassengers = checkSchedule.numberofpassengers;


      
            Database1Entities1 db = new Database1Entities1();


            int currentlybooked = countbookedinschedule(fullrecord.scheduleindex);
            int booklimit = (int)db.Schedules.FirstOrDefault(g => g.schedule_id == fullrecord.scheduleindex).booklimit;
           



            if((currentlybooked+ fullrecord.numberofpassengers)>= booklimit)
            {
                tempclass.modalstate = "the schedule is fully booked , sorry for inconveniece";
                return Redirect("Index");
            }

          

            var tempsched = db.Schedules.FirstOrDefault(m => m.schedule_id == tempclass.indextemp);
      
            if (tempsched != null)
            {

                Route temproute = db.Routes.FirstOrDefault(n => n.route_id == tempsched.route_id);

                ViewBag.locationfrom = temproute.locationfrom;
                ViewBag.locationto = temproute.locationto;
                ViewBag.terminalfrom = temproute.terminalfrom;
                ViewBag.terminalto = temproute.terminalto;

                ViewBag.departuretime = tempsched.departuretime;
                ViewBag.arrivaltime = tempsched.arrivaltime;
                ViewBag.transportname = tempsched.transport_name;
                ViewBag.priceregular = temproute.updatedpriceregular_;
                ViewBag.pricetourist = temproute.updatedpricetourist;
                ViewBag.pricebusiness = temproute.updatedpricebusiness;
                ViewBag.pricebusiness2 = temproute.updatedpricebusiness2;
                ViewBag.mode1 = temproute.mode;
                
            }  

            return View();

        }





        [HttpPost]
        public ActionResult Cargobooking(ycheckSchedule checkschedule2)
        {


            if (CurrentUser.userId == null)
            {
                return Redirect("Login");
            }


            ViewBag.Currentuser = CurrentUser.username; ///kailangan to

            Database1Entities1 db = new Database1Entities1();

           
            var tempsched = db.Schedules.FirstOrDefault(m => m.schedule_id == tempclass.indextemp);
            Route temproute = db.Routes.FirstOrDefault(n => n.route_id == tempsched.route_id);
            ViewBag.locationfrom = temproute.locationfrom;
            ViewBag.locationto = temproute.locationto;
            ViewBag.terminalfrom = temproute.terminalfrom;
            ViewBag.terminalto = temproute.terminalto;

            ViewBag.departuretime = tempsched.departuretime;
            ViewBag.arrivaltime = tempsched.arrivaltime;
            ViewBag.transportname = tempsched.transport_name;
            ViewBag.pricecargo = temproute.updatedpricecargo;
            ViewBag.priceregular = temproute.updatedpriceregular_;
            ViewBag.pricetourist = temproute.updatedpricetourist;
            ViewBag.pricebusiness = temproute.updatedpricebusiness;
            ViewBag.pricebusiness2 = temproute.updatedpricebusiness2;
            ViewBag.mode1 = temproute.mode;



            double temp1 = 0;

            try
            {
                temp1 = (double)(checkschedule2.kilo * temproute.updatedpricecargo);
            }
            catch
            {
                
            }
         
            double tax = temp1 * .12;
            double gross = temp1 + tax;
            ViewBag.net1 = temp1;
            ViewBag.tax1 = tax;
            ViewBag.gross1 = gross;
            fullrecord.inputkilo = checkschedule2.kilo;
            return View();
        }


      





       


        [HttpPost]
        public ActionResult additionaldetails(yGroup1 thenewgroup)
        {
            if (CurrentUser.userId == null)
            {
                return Redirect("Login");
            }

            fullrecord.thenewgroup1 = thenewgroup;
            fullrecord.datecreated = DateTime.Now.ToString();

            // payment option, number of passegner  etc.

            return additionaldetails("regular");
        }



 

        public ActionResult additionaldetails(string typeofpayment)
        {
            ViewBag.Currentuser = CurrentUser.username;
            ViewBag.currentEmail = fullrecord.clientEmail;
            ViewBag.currentContact = fullrecord.clientContact;
            
            fullpricing fullpricing1 = adjustprices(typeofpayment);


            if(fullrecord.payment_option== null && typeofpayment== null)
            {
                typeofpayment = "regular";
            }else if(typeofpayment != null)
            {
                fullrecord.payment_option = typeofpayment;
            }

           

            ViewBag.gross1 = fullpricing1.grossprice;
            ViewBag.pph1 = fullpricing1.priceperhead;
            ViewBag.sen1 = fullpricing1.seniordiscount;
            ViewBag.notax1 = fullpricing1.notaxprice;
            ViewBag.tax1 = fullpricing1.taxamount;
            ViewBag.notax1 = fullpricing1.notaxprice;
            ViewBag.final1 = fullpricing1.finalprice;

            if(tempclass.mustaccept!= null)
            {
                ViewBag.mustaccept = tempclass.mustaccept;
                tempclass.mustaccept = null;
            }



            Database1Entities1 db = new Database1Entities1();

            var tempsched = db.Schedules.FirstOrDefault(m => m.schedule_id == tempclass.indextemp);

            if (tempsched != null)
            {

                Route temproute = db.Routes.FirstOrDefault(n => n.route_id == tempsched.route_id);

                ViewBag.locationfrom = temproute.locationfrom;
                ViewBag.locationto = temproute.locationto;
                ViewBag.terminalfrom = temproute.terminalfrom;
                ViewBag.terminalto = temproute.terminalto;

                ViewBag.departuretime = tempsched.departuretime;
                ViewBag.arrivaltime = tempsched.arrivaltime;
                ViewBag.transportname = tempsched.transport_name;
                ViewBag.priceregular = temproute.updatedpriceregular_;
                ViewBag.pricetourist = temproute.updatedpricetourist;
                ViewBag.pricebusiness = temproute.updatedpricebusiness;
                ViewBag.pricebusiness2 = temproute.updatedpricebusiness2;
                ViewBag.mode1 = temproute.mode;

                fullrecord.priceregular = temproute.updatedpriceregular_;
                fullrecord.pricetouristclass = temproute.updatedpricetourist;
                fullrecord.pricebusinessclass = temproute.updatedpricebusiness;
                fullrecord.pricebusinessclass2 = temproute.updatedpricebusiness2;


                return View();
            }


            return View();
        }


      




        private fullpricing adjustprices(string mmm)
        {
          

            fullpricing temp = new fullpricing();
            double? rawprice; 
            switch (mmm)
            {
                case "regular": rawprice = fullrecord.priceregular;
                    break;
                case "tourist": rawprice = fullrecord.pricetouristclass;
                    break;
                case "business": rawprice = fullrecord.pricebusinessclass;
                    break;
                case "business2": rawprice = fullrecord.pricebusinessclass2;
                    break;
                default: rawprice = fullrecord.priceregular; 
                    break;
            }


            
            temp.priceperhead = rawprice;
            temp.grossprice = rawprice * fullrecord.numberofpassengers;
            temp.notaxprice = totalfare(fullrecord.thenewgroup1) * rawprice;
            temp.seniordiscount = fullrecord.seniourdiscountmultiplier * temp.grossprice;          
            temp.taxamount = temp.notaxprice * .12;
            temp.finalprice = temp.notaxprice + temp.taxamount;

            return temp;
            
        }



      

        private double totalfare(yGroup1 thenewgroup)
        {
            fullrecord.seniourdiscountmultiplier = 0;
            fullrecord.numberofseniours = 0;
            double faremultiplier = include1(thenewgroup.age1)+ include1(thenewgroup.age2)+ include1(thenewgroup.age3)+ include1(thenewgroup.age4);
            
            return faremultiplier;
        }

        private double include1(int? perosn1)
        {

            if(perosn1 == null)
            {
                return 0;
            }else if(perosn1>= 60)
            {
                fullrecord.seniourdiscountmultiplier += .2;
                fullrecord.numberofseniours++;
                return .8;
             
            }
            else
            {
                return 1;
            }
        }


        [HttpPost]
        public ActionResult finishbooking(yadditionaldetails temp1)
        {

            if(temp1.terms== false)
            {
                tempclass.mustaccept = "must accept terms and agreements";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
         
          
            if (temp1.clientEmail != null )
            {  
                fullrecord.clientEmail = temp1.clientEmail;
            }
            if (temp1.clientContact != null)
            {
                fullrecord.clientContact = temp1.clientContact;
            }
          
          
            createBooking(fullrecord.thenewgroup1);

            return Redirect("mybookingsandProfile");
        }





        [HttpPost]
        public ActionResult finishcargobooking(yCargobooking  cargobook)
        {
            Database1Entities1 db = new Database1Entities1();
            Cargobooking temp1 = new Cargobooking();
            temp1.datecreated = DateTime.Now.ToString();
            temp1.itemweight = fullrecord.inputkilo;
            temp1.user_id = CurrentUser.userId;
            temp1.status = "pending";
            temp1.schedule_id = tempclass.indextemp;

            db.Cargobookings.Add(temp1);
            db.SaveChanges();
        
            tempclass.modalstate = "cargo booking successful";
            return Redirect("mycargobookings");
        }






  

        private void createBooking(yGroup1 temp1)
        {

            Database1Entities1 db = new Database1Entities1();

         

            Booking newbooking = new Booking();
            newbooking.schedule_id = fullrecord.scheduleindex;    //schedule id
            newbooking.numberofpassengers = fullrecord.numberofpassengers;

          

            newbooking.clientemail = fullrecord.clientEmail;


            Group newgroup = new Group();
            newgroup.datecreated = DateTime.Now.ToString();
            db.Groups.Add(newgroup);
            db.SaveChanges();


            int yy = db.Groups.OrderByDescending(m => m.groupId).First().groupId;
            createindividual(yy, temp1.firstname1, temp1.surname1, temp1.middle1, temp1.age1, temp1.isEnsured1, temp1.isVaxxed1);
            createindividual(yy, temp1.firstname2, temp1.surname2, temp1.middle2, temp1.age2, temp1.isEnsured2, temp1.isVaxxed2);
            createindividual(yy, temp1.firstname3, temp1.surname3, temp1.middle3, temp1.age3, temp1.isEnsured3, temp1.isVaxxed3);
            createindividual(yy, temp1.firstname4, temp1.surname4, temp1.middle4, temp1.age4, temp1.isEnsured4, temp1.isVaxxed4);


            newbooking.passenger_info_id = yy;   //passgenger group id
            newbooking.numberofpassengers = fullrecord.numberofpassengers; //  number of passenger  
            newbooking.numberofseniour = fullrecord.numberofseniours; // number of seniors
            newbooking.paymentoption = fullrecord.payment_option;  //payment option
            newbooking.paymentstatus = "pending";   //payment status
            newbooking.clientemail = fullrecord.clientEmail;
            newbooking.clientnumber = fullrecord.clientContact;
            newbooking.user_id = CurrentUser.userId;
            newbooking.datecreated = DateTime.Now.ToString();
           

            db.Bookings.Add(newbooking);
            db.SaveChanges();
            tempclass.modalstate = "Successfully booked";


            int temptemp = countbookedinschedule(fullrecord.scheduleindex);
            Schedule schedsched = db.Schedules.FirstOrDefault(i => i.schedule_id == fullrecord.scheduleindex);
            if (temptemp >= schedsched.booklimit)
            {
                schedsched.sched_status = "fully booked";      
                db.SaveChanges();
            }


        }






        private void createindividual(int? groupnumber, string firstname, string lastname, string middle, int? age, bool isensured, bool isvacced)
        {
            Database1Entities1 db = new Database1Entities1();
            if(firstname != null)
            {
  
                PassengerInfo tempt = new PassengerInfo();
    
                tempt.groupnumber = groupnumber;
             
                tempt.firstname = firstname;
                tempt.surname = lastname;
                tempt.middle = middle;
                tempt.age = age;
                tempt.isEnsured = isensured;
                tempt.isVaxxed = isvacced;
                db.PassengerInfoes.Add(tempt);         
                db.SaveChangesAsync();

            }
        }



        public ActionResult mybookingsandProfile()
        {

            ViewBag.Currentuser = CurrentUser.username;


            if(tempclass.modalstate!= null)
            {
                ViewBag.modalstate = tempclass.modalstate;
                tempclass.modalstate = null;
            }

            Database1Entities1 db = new Database1Entities1();



            try
            {
                User ako = db.Users.FirstOrDefault(m => m.userid == CurrentUser.userId);
                if (ako != null)
                {
                    ViewBag.username1 = ako.username;
                    ViewBag.email1 = ako.email;
                    ViewBag.fname = ako.fname;
                    ViewBag.lname = ako.lname;
                    ViewBag.city = ako.city;
                    ViewBag.contact = ako.contactnumber;
                }
                else
                {
                    return Redirect("Index");
                }

            }
            catch (NullReferenceException)
            {
                ViewBag.username1 = "no username";
                ViewBag.email1 = "no user email";
                ViewBag.fname = "n/a";
                ViewBag.lname = "n/a";
                ViewBag.city = "n/a";
                ViewBag.contact = "n/a";
            }


            var bookingsko = db.Bookings.Where(b => b.user_id == CurrentUser.userId && b.paymentstatus != "cancelled").Join(db.Schedules, s => s.schedule_id, r => r.schedule_id, (book, sched) => new { book, sched }).Join(db.Routes, t => t.sched.route_id, r => r.route_id, (booksched, route) => new schedulelist()
            {
                locationfrom = route.locationfrom,
                locationto = route.locationto,
                departuretime = booksched.sched.departuretime,
                arrivaltime = booksched.sched.arrivaltime,
                numberofpassenger = booksched.book.numberofpassengers,
                paymenttype = booksched.book.paymentoption,
                terminalfrom = route.terminalfrom,
                book_id= booksched.book.booklog
            }) ;


            ViewBag.bookingsko = bookingsko;
            
            return View();

        }





        public ActionResult allschedule()
        {

            ViewBag.Currentuser = CurrentUser.username;


            Database1Entities1 db = new Database1Entities1();


         


           


            ViewBag.schedland = get_schedule("land");
            ViewBag.schedair = get_schedule("air");
            ViewBag.schedwater = get_schedule("water");



            return View();
        }




        public ActionResult editprofile()
        {
            ViewBag.Currentuser = CurrentUser.username;

            Database1Entities1 db = new Database1Entities1();


            try
            {
                User ako = db.Users.FirstOrDefault(m => m.userid == CurrentUser.userId);
                if (ako != null)
                {
                    ViewBag.email1 = ako.email;       
                    ViewBag.fname = ako.fname;
                    ViewBag.lname = ako.lname;
                    ViewBag.city = ako.city;
                    ViewBag.contact = ako.contactnumber;

                }
                else
                {
                    return Redirect("mybookingsandProfile");
                }

            }
            catch (NullReferenceException)
            {
                return Redirect("Index");
            }

            var bookingsko = db.Bookings.Where(b => b.user_id == CurrentUser.userId && b.paymentstatus != "cancelled").Join(db.Schedules, s => s.schedule_id, r => r.schedule_id, (book, sched) => new { book, sched }).Join(db.Routes, t => t.sched.route_id, r => r.route_id, (booksched, route) => new schedulelist()
            {
                locationfrom = route.locationfrom,
                locationto = route.locationto,
                departuretime = booksched.sched.departuretime,
                arrivaltime = booksched.sched.arrivaltime,
                numberofpassenger = booksched.book.numberofpassengers,
                paymenttype = booksched.book.paymentoption,
                terminalfrom = route.terminalfrom,
                book_id = booksched.book.booklog
            });


            ViewBag.bookingsko = bookingsko;

            return View();

        }

        [HttpPost]
        public ActionResult tryedit(yeditprofile newprofile)
        {
            Database1Entities1 db = new Database1Entities1();


            User temp1 = db.Users.FirstOrDefault(m => m.userid == CurrentUser.userId);


           if(temp1!= null)
            {
                temp1.email = newprofile.email;
                CurrentUser.useremail = newprofile.email;
                fullrecord.clientEmail = newprofile.email;
                temp1.password = newprofile.password;
                temp1.fname = newprofile.fname;
                temp1.lname = newprofile.lname;
                temp1.city = newprofile.city;
                temp1.contactnumber = newprofile.contactnumber.ToString();

                db.SaveChanges();
            
                tempclass.modalstate = "Successfully edited profile";

            }
            else
            {
                MessageBox.Show("something wrogn here");
            }

            return Redirect("mybookingsandProfile");
        }



        public ActionResult mycargobookings()
        {
            ViewBag.Currentuser = CurrentUser.username;

            if (tempclass.modalstate != null)
            {
                ViewBag.modalstate = tempclass.modalstate;
                tempclass.modalstate = null;
            }


            Database1Entities1 db = new Database1Entities1();

            try
            {
                User ako = db.Users.FirstOrDefault(m => m.userid == CurrentUser.userId);
                if (ako != null)
                {
                    ViewBag.username1 = ako.username;
                    ViewBag.email1 = ako.email;
                }
                else
                {
                    ViewBag.username1 = "no username";
                    ViewBag.email1 = "no user email";
                }

            }
            catch (NullReferenceException)
            {
                ViewBag.username1 = "no username";
                ViewBag.email1 = "no user email";
            }






            var cargobookingsko = db.Cargobookings.Where(b => b.user_id == CurrentUser.userId && b.status == "pending").Join(db.Schedules, s => s.schedule_id, r => r.schedule_id, (book, sched) => new { book, sched }).Join(db.Routes, t => t.sched.route_id, r => r.route_id, (booksched, route) => new cargolist()
            {
                departure = booksched.sched.departuretime,
                arrival  = booksched.sched.arrivaltime,
                locationfrom = route.locationfrom,
                locationto = route.locationto,
                itemweight = booksched.book.itemweight,
                status = booksched.book.status,
                cargo_id = booksched.book.Id
            }).ToList();


            ViewBag.cargoko = cargobookingsko;


            return View();

        }



        public ActionResult cancelbooking(int index1)
        {
            Database1Entities1 db = new Database1Entities1();
            Booking temp = db.Bookings.FirstOrDefault(m => m.booklog == index1);
            if(temp!= null)
            {
                temp.paymentstatus = "cancelled";
                db.SaveChanges();
                tempclass.modalstate = "Booking cancelled";
            }
            else
            {
                tempclass.modalstate ="Something wrong occured";
            }

            return Redirect("mybookingsandProfile");
        }



        public ActionResult cancelcargo(int index1)
        {
            Database1Entities1 db = new Database1Entities1();
            Cargobooking temp = db.Cargobookings.FirstOrDefault(k => k.Id ==index1);
            if (temp != null)
            {
                temp.status = "cancelled";
                db.SaveChanges();
                tempclass.modalstate = "Booking cancelled";
            }
            else
            {
                tempclass.modalstate = "Something wrong occured";
            }


            return Redirect("mycargobookings");
        }


        private List<schedulelist> get_schedule(string mode1)
        {

            Database1Entities1 db = new Database1Entities1();

            List<schedulelist>  temp= db.Schedules.Join(db.Routes.Where(m => m.mode == mode1), t => t.route_id, u => u.route_id, (sched, route) => new schedulelist()
            {
                locationfrom = route.locationfrom,
                locationto = route.locationto,
                terminalfrom = route.terminalfrom,
                terminalto = route.terminalto,
                departuretime = sched.departuretime,
                arrivaltime = sched.arrivaltime,
                priceregular2 = route.updatedpriceregular_.ToString(),
                sched_status = sched.sched_status
            }).ToList();

            return temp;

        }


        private int countbookedinschedule(int? schedindx)
        {

            if(schedindx == null)
            {
                return 0;
            }
            Database1Entities1 db = new Database1Entities1();
            int numberbookings = db.PassengerInfoes.Join(db.Groups, b => b.groupnumber, e => e.groupId, (info, group) => new { info, group }).Join(db.Bookings.Where( i => i.schedule_id == schedindx && i.paymentstatus != "cancelled") , y => y.group.groupId , z => z.passenger_info_id , (infogroup, book)=> new { infogroup, book}).Count();
            return numberbookings;
        }











    }
}