using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Landmark
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Name of the landmark")]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }

        [Required]
        [Display(Name = "Hours of Operation")]
        public string HoursOfOperation { get; set; }

        [Display(Name = "Image file name")]
        public string ImageLocation { get; set; }

        //public List<LandmarkReview> Reviews { get; set; }

        public override bool Equals(object obj)
        {
            return (this.ID == ((Landmark)obj).ID);
        }
    }
}
