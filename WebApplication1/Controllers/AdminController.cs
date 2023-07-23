using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{




   


    public static class CurrentUser
    {
        public static Nullable<int> userId { get; set; }
        public static string usertype { get; set; } = "";
        public static string username { get; set; } 
        public static string useremail { get; set; }



        public static string redirect11()
        {

         

            if(userId != null && usertype.Equals("admin"))
            {
                return "admin";
            }
            else if (userId != null && usertype.Equals("client"))
            {
                return "client";
            }
            else
            {
                return "no user";
            }
        }



    }



    public static class Currenttab
    {
        public static string activetab { get; set; } = "dashboard";
        public static string modalmessage { get; set; }
    }


    public partial class booktable
    {

        public string datecreated { get; set; }
        public string travelfrom { get; set; }
        public string travelto { get; set; }  
        public string email { get; set; }
        public int? numberofpassenger { get; set; } 
        public string accomodation { get; set; }
        public int? bookid { get; set; }
        public string transportname { get; set; }
        public string status { get; set; }

    } 

    public partial class cargotable
    {
        public string datecreated { get; set; }
        public string email { get; set; }
        public double? itemweight { get; set; }
        public string departure { get; set; }
        public string arrival { get; set; }
        public string terminalto { get; set; }
        public string terminalfrom { get; set; }
        public string status { get; set; }
    }





    public class AdminController : Controller
    {



        [ChildActionOnly]
        public ActionResult getusername()
        {
            MessageBox.Show("user id = " + CurrentUser.username );
            //return new ContentResult { Content = CurrentUser.userId.ToString()};
            return new ContentResult { Content = CurrentUser.username };
        }



        // GET: Admin
        public ActionResult Dashboard()
        {


            ViewBag.currenttab = "dashboard";
            Database1Entities1 db = new Database1Entities1();

           
            ViewBag.routaland = getroutelist("land");
            ViewBag.routaair = getroutelist("air");
            ViewBag.routasea = getroutelist("sea");

            ViewBag.transportland = gettransportlist("land");
            ViewBag.transportsea = gettransportlist("sea");
            ViewBag.transportair = gettransportlist("air");


            if (Currenttab.modalmessage != null)
            {
                ViewBag.modalstate = Currenttab.modalmessage;
                Currenttab.modalmessage = null;
            }


            switch (CurrentUser.redirect11())
            {
                case "admin":

                    ViewBag.schedland = GetSchedulelist("land");
                    ViewBag.schedwater = GetSchedulelist("sea");
                    ViewBag.schedair = GetSchedulelist("air");

                    ViewBag.booknumber1 = db.Bookings.Where(h => h.paymentstatus== "pending").Count();
                    ViewBag.booknumber2 = db.Bookings.Where(h => h.paymentstatus== "approved").Count();
                    ViewBag.booknumber3 = db.Bookings.Where(h => h.paymentstatus== "cancelled").Count();

                    ViewBag.cargocount1 = db.Cargobookings.Where(h=> h.status == "pending").Count();
                    ViewBag.cargocount2 = db.Cargobookings.Where(h=> h.status == "approved").Count();
                    ViewBag.cargocount3 = db.Cargobookings.Where(h=> h.status == "cancelled").Count();


                    ViewBag.usercount = db.Users.Count();


                    return View();

                case "client": return RedirectToAction("Index", "Home", null);
                default: return RedirectToAction("Login", "Home", null);

            }

        }

       


        public ActionResult PageRedirect()
        {
            switch (CurrentUser.redirect11())
            {
                case "admin": return RedirectToAction("Dashboard", "Admin", null);
                case "client": return RedirectToAction("Index", "Home", null);
                default: return RedirectToAction("Login", "Home", null);

            }

        }







       


        public ActionResult  deletesched(int? x)
        {
            switch (CurrentUser.redirect11())
            {
                case "admin":
                    break;
                  

                case "client": return RedirectToAction("Index", "Home", null);
                default: return RedirectToAction("Login", "Home", null);

            }


            Database1Entities1 db = new Database1Entities1();

            int temp2 = db.Bookings.Where(n => n.schedule_id == x).Count();
            if (temp2 > 0)
            {
                Currenttab.modalmessage = "cannot delete this schedule because it is recorded at bookings";
                return Redirect("Dashboard");
            }

            Schedule schedule = db.Schedules.FirstOrDefault(n => n.schedule_id == x);

            if (schedule != null)
            {
                Currenttab.modalmessage = "successfully deleted schedule";
                db.Schedules.Remove(schedule);
                db.SaveChanges();
            }
          
            return Redirect("Dashboard");
        }


      







      

        [HttpPost]
        public ActionResult addschedule2(ySchedule yy )
        {
            Database1Entities1 db = new Database1Entities1();

            Schedule tempsched = new Schedule();

            tempsched.route_id = yy.route_id;
            tempsched.transport_name = yy.transport_name;
            tempsched.departuretime = yy.departuretime;
            tempsched.arrivaltime = yy.arrivaltime;
            tempsched.sched_status = "closed";
            tempsched.booklimit = 100;

            db.Schedules.Add(tempsched);
            db.SaveChanges();


            Currenttab.modalmessage = "successfully added schedule";
            return Redirect("Dashboard");
        }


       

        public ActionResult Viewbookings(int? x = null)
        {
            switch (CurrentUser.redirect11())
            {
                case "admin":
                    break;
                case "client": return RedirectToAction("Index", "Home", null);
                default: return RedirectToAction("Login", "Home", null);

            }

            ViewBag.currenttab = "bookings";


            Database1Entities1 db = new Database1Entities1();
            var bookinglist = db.Bookings.Join(db.Schedules, s => s.schedule_id, f => f.schedule_id, (book, sched) => new { book, sched }).Join(db.Routes, r => r.sched.route_id, t => t.route_id, (booksched, route) => new booktable() {
                        datecreated= booksched.book.datecreated,
                        travelfrom = route.locationfrom,
                        travelto = route.locationto,
                        email = booksched.book.clientemail,
                        numberofpassenger = booksched.book.numberofpassengers,
                        accomodation = booksched.book.paymentoption,
                        bookid = booksched.book.booklog,
                        status = booksched.book.paymentstatus
          
            }).ToList();
    
            ViewBag.bookinglist = bookinglist;

            if (x != null)
            {
                Booking thisbook = db.Bookings.FirstOrDefault(m => m.booklog == x);
                ViewBag.thisdatecreated = thisbook.datecreated;
                ViewBag.thisclientemail = thisbook.clientemail;
                ViewBag.thisclientcontact = thisbook.clientnumber;
                ViewBag.thispaymentoption = thisbook.paymentoption;
                ViewBag.paymentstatus = thisbook.paymentstatus;

                Schedule thissched = db.Schedules.FirstOrDefault(m => m.schedule_id == thisbook.schedule_id);
                Route thisroute = db.Routes.FirstOrDefault(m => m.route_id == thissched.route_id);
                ViewBag.travel = thisroute.locationfrom + " to " + thisroute.locationto;
                ViewBag.terminal = thisroute.terminalfrom + " to " + thisroute.terminalto;
                ViewBag.departure = thissched.departuretime;
                var tempgroup = db.PassengerInfoes.Where(m => m.groupnumber == thisbook.passenger_info_id).ToList();
                ViewBag.tempgroup = tempgroup;

                ViewBag.modalstate = "viewing_info";
            }



            return View();


        }



        public ActionResult ViewCargobooking()
        {
            switch (CurrentUser.redirect11())
            {
                case "admin":
                    break;


                case "client": return RedirectToAction("Index", "Home", null);
                default: return RedirectToAction("Login", "Home", null);

            }

            ViewBag.currenttab = "cargobookings";

            Database1Entities1 db = new Database1Entities1();


            var cargotable = db.Cargobookings.Join(db.Schedules, m => m.schedule_id, b => b.schedule_id, (cargo, sched) => new { cargo, sched }).Join(db.Routes, g => g.sched.route_id, y => y.route_id, (cargosched, route) => new { cargosched, route }).Join(db.Users, o => o.cargosched.cargo.user_id, u => u.userid, (cargoschedroute, User1) => new cargotable() { 
                           datecreated = cargoschedroute.cargosched.cargo.datecreated,
                           email= User1.email ,
                           itemweight = cargoschedroute.cargosched.cargo.itemweight,
                           terminalfrom = cargoschedroute.route.terminalfrom,
                           terminalto = cargoschedroute.route.terminalto,
                           departure = cargoschedroute.cargosched.sched.departuretime,
                           arrival = cargoschedroute.cargosched.sched.arrivaltime,
                           status = cargoschedroute.cargosched.cargo.status
            
            }).ToList();

            ViewBag.cargotable = cargotable;



            return View();

        }


        [HttpPost]
        public ActionResult searchroute(yRoutepage temp)
        {
            
            if(temp.searchtxt != null)
            {
         
                return RedirectToAction("Route", new { searchtxt = temp.searchtxt});
            }
            else
            {
                return Redirect("Route");
            }
        }

        


       

        public ActionResult Route(int? index1, string searchtxt)
        {
            switch (CurrentUser.redirect11())
            {
                case "admin":
                    break;


                case "client": return RedirectToAction("Index", "Home", null);
                default: return RedirectToAction("Login", "Home", null);

            }

            ViewBag.currenttab = "routes";
            Database1Entities1 db = new Database1Entities1();


            if (Currenttab.modalmessage != null)
            {
                ViewBag.modalstate = Currenttab.modalmessage;
                Currenttab.modalmessage = null;
            }

            if (searchtxt == null)
            {
                ViewBag.routes = db.Routes.ToList();

            }
            else {
                
                ViewBag.routes = db.Routes.Where(m=> m.locationfrom == searchtxt || m.locationto == searchtxt).ToList();
                
            }

            ViewBag.transports = db.transports.ToList();
            ViewBag.terminal = db.Terminals.ToList();
            // routetable, transport table



            //for routes
            var temp1 = db.Terminals.ToList();
            List<SelectListItem> terminallist = new List<SelectListItem>();
           foreach(var yy in temp1)
            {
                string temp12 = yy.terminal_location + " (" + yy.terminal_name + ")";
                terminallist.Add(new SelectListItem {Text = temp12 , Value = yy.terminal_id.ToString() });
            }
            ViewBag.terminallist = terminallist;


            List<string> tempmode1 = new List<string>() {"land", "sea", "air"};
            ViewBag.tempmode1 = tempmode1;

            List<string> transportstatus = new List<string>() { "active", "disabled" };
            ViewBag.transportstatus = transportstatus;
            
        

            if(index1 != null)
            {
                Route temproute = db.Routes.FirstOrDefault(m => m.route_id == index1);           
                ViewBag.routeindex = temproute.route_id;
                ViewBag.priceregular = temproute.updatedpriceregular_;
                ViewBag.pricetourist = temproute.updatedpricetourist;
                ViewBag.pricebusiness = temproute.updatedpricebusiness;
                ViewBag.pricebusiness2 = temproute.updatedpricebusiness2;
                ViewBag.pricecargo = temproute.updatedpricecargo;
                ViewBag.modalstate = "updating_route";

            }

            return View();
        }



        [HttpPost]
        public ActionResult saveroute(yRoutepage routepage)
        {

           
            if (routepage.terminalfrom == routepage.terminalto)
            {
               
                MessageBox.Show("locations must not be the same");
                return Redirect("Route");
            }




            Database1Entities1 db = new Database1Entities1();
            Route newroute = new Route();
            Route backroute = new Route();


        
            Terminal tempterminal1 = db.Terminals.FirstOrDefault(p => p.terminal_id.ToString() == routepage.terminalfrom);
            newroute.locationfrom = tempterminal1.terminal_location;
            newroute.terminalfrom = tempterminal1.terminal_name;
            backroute.terminalto = newroute.terminalfrom;
            backroute.locationto = newroute.locationfrom;

         
            Terminal tempterminal2 = db.Terminals.FirstOrDefault(p => p.terminal_id.ToString() == routepage.terminalto);

            newroute.locationto = tempterminal2.terminal_location;
            newroute.terminalto = tempterminal2.terminal_name;
            backroute.locationfrom = newroute.locationto;
            backroute.terminalfrom = newroute.terminalto;


            newroute.updatedpriceregular_ = routepage.price1;
            newroute.updatedpricetourist = routepage.price2;
            newroute.updatedpricebusiness = routepage.price3;
            newroute.updatedpricebusiness2 = routepage.price4;
            newroute.updatedpricecargo = routepage.pricecargo;
            newroute.mode = routepage.routemode;

            backroute.updatedpriceregular_ = routepage.price1;
            backroute.updatedpricetourist = routepage.price2;
            backroute.updatedpricebusiness = routepage.price3;
            backroute.updatedpricebusiness2 = routepage.price4;
            backroute.updatedpricecargo = routepage.pricecargo;
            backroute.mode = routepage.routemode;


            // check if the route exists

            Route checkroute1 = db.Routes.FirstOrDefault(m => m.locationfrom == newroute.locationfrom && m.locationto == newroute.locationto && m.terminalfrom == newroute.terminalfrom && m.terminalto == newroute.terminalto);
            Route checkroute2 = db.Routes.FirstOrDefault(m => m.locationfrom == backroute.locationfrom && m.locationto == backroute.locationto && m.terminalfrom == backroute.terminalfrom && m.terminalto == newroute.terminalto);

            if(checkroute1!= null || checkroute2!= null)
            {
                Currenttab.modalmessage = "this route already exists";
                return Redirect("Route");
            }



            db.Routes.Add(newroute);
            db.SaveChanges();
            db.Routes.Add(backroute);
            db.SaveChanges();


            Currenttab.modalmessage = "Route successfully added";
            return Redirect("Route");
        }



        [HttpPost]
        public ActionResult saveterminal(yRoutepage routepage)
        {

            Database1Entities1 db = new Database1Entities1();

            Terminal tempterminal = new Terminal();
            tempterminal.terminal_location = routepage.terminallocation;
            tempterminal.terminal_name = routepage.terminalname;
            db.Terminals.Add(tempterminal);
            db.SaveChanges();

            Currenttab.modalmessage = "Terminal successfully added";
            return Redirect("Route");
        }





        [HttpPost]
        public ActionResult savetransport(yRoutepage routepage)
        {
            Database1Entities1 db = new Database1Entities1();
            transport temp = new transport();
            temp.transport_name = routepage.transportname;
            temp.transport_type = routepage.trasnporttype;
            temp.transport_status = routepage.transportstatus;

            db.transports.Add(temp);
            db.SaveChanges();

            Currenttab.modalmessage = "Transport unit successfully added";
            return Redirect("Route");
        }



        public ActionResult toggletransportstats(int? index1)
        {
            Database1Entities1 db = new Database1Entities1();
            transport temp1 = db.transports.FirstOrDefault(i => i.transport_id == index1);

            if(temp1!= null)
            {
                if(temp1.transport_status== "active")
                {
                    temp1.transport_status = "disabled";
                }
                else
                {
                    temp1.transport_status = "active";

                }

                db.SaveChanges();
            }

            return Redirect("Route");
        }

        public ActionResult toggleopen(int? index1)
        {
            Database1Entities1 db = new Database1Entities1();
            Schedule tempsched = db.Schedules.FirstOrDefault(v => v.schedule_id == index1);

            if(tempsched != null)
            {
                if(tempsched.sched_status== "open")
                {
                    tempsched.sched_status = "closed";
                }
                else if( tempsched.sched_status =="closed")
                {
                    tempsched.sched_status = "open";
                }
                else
                {

                }
                db.SaveChanges();
                
            }
            return Redirect("Dashboard");

        }




        private List<SelectListItem> getroutelist(string mode1)
        {
            Database1Entities1 db = new Database1Entities1();
            var routaA = db.Routes.Where(u => u.mode == mode1).Select(v => new { v.route_id, v.locationfrom, v.locationto, v.mode }).Distinct().ToList();
            List<SelectListItem> routetemp = new List<SelectListItem>();


            foreach (var tt in routaA)
            {
                string temp = tt.locationfrom + " to " + tt.locationto + " (" + tt.mode + ")";
                routetemp.Add(new SelectListItem { Text = temp, Value = tt.route_id.ToString() });
            }

            return routetemp;
        }


        private List<SelectListItem> gettransportlist(string mode1)
        {
            Database1Entities1 db = new Database1Entities1();
            var transportA = db.transports.Where(u => u.transport_type == mode1).Select(o => new { o.transport_name, o.transport_type }).Distinct().ToList();
            List<SelectListItem> transporttemp = new List<SelectListItem>();

            foreach (var tt in transportA)
            {
                string temp = tt.transport_name + " (" + tt.transport_type + ")";
                transporttemp.Add(new SelectListItem { Text = temp, Value = tt.transport_name });
            }
            return transporttemp;

        }

        private IEnumerable<schedulelist> GetSchedulelist(string mode1)
        {
            Database1Entities1 db = new Database1Entities1();

            IEnumerable<schedulelist> temp = db.Schedules.Join(db.Routes.Where(m => m.mode == mode1), t => t.route_id, u => u.route_id, (sched, route) => new schedulelist()
            {
                locationfrom = route.locationfrom,
                locationto = route.locationto,
                terminalfrom = route.terminalfrom,
                terminalto = route.terminalto,
                departuretime = sched.departuretime,
                arrivaltime = sched.arrivaltime,
                transportname = sched.transport_name,
                sched_id = sched.schedule_id,
                sched_status = sched.sched_status

            });

            return temp; 
        }



        [HttpPost]
        public ActionResult updaterouteprices(yRoutepage routeitem)
        {

            Database1Entities1 db = new Database1Entities1();
            Route temproute = db.Routes.FirstOrDefault(m => m.route_id == routeitem.routeId);
            if(temproute != null)
            {
                Route temproute2 = db.Routes.FirstOrDefault(m => m.locationfrom == temproute.locationto && m.locationto == temproute.locationfrom);


                temproute.updatedpriceregular_ = routeitem.price1;
                temproute.updatedpricetourist = routeitem.price2;
                temproute.updatedpricebusiness = routeitem.price2;
                temproute.updatedpricebusiness2 = routeitem.price4;
                temproute.updatedpricecargo = routeitem.pricecargo;

                temproute2.updatedpriceregular_ = routeitem.price1;
                temproute2.updatedpricetourist = routeitem.price2;
                temproute2.updatedpricebusiness = routeitem.price2;
                temproute2.updatedpricebusiness2 = routeitem.price4;
                temproute2.updatedpricecargo = routeitem.pricecargo;

                
                db.SaveChanges();
                Currenttab.modalmessage = "Prices updated";

            }
            else
            {
                MessageBox.Show("something wrong here");
            }

            return Redirect("Route");
        }









    }
}