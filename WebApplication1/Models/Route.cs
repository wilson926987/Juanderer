//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Route
    {
        public int route_id { get; set; }
        public string locationfrom { get; set; }
        public string locationto { get; set; }
        public string terminalfrom { get; set; }
        public string terminalto { get; set; }
        public Nullable<double> updatedpriceregular_ { get; set; }
        public Nullable<double> updatedpricetourist { get; set; }
        public Nullable<double> updatedpricebusiness { get; set; }
        public Nullable<double> updatedpricebusiness2 { get; set; }
        public string mode { get; set; }
        public Nullable<double> updatedpricecargo { get; set; }
    }
}