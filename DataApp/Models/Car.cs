using System;

namespace DataApp.Models
{
    public class Car
    {
        public int RowId { get; set; }
        public string region { get; set; }
        public long? price { get; set; }
        public int? year { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string condition { get; set; }
        public string cylinders { get; set; }
        public string fuel { get; set; }
        public int? odometer { get; set; }
        public string title_status { get; set; }
        public string transmission { get; set; }
        public string VIN { get; set; }
        public string drive { get; set; }
        public string size { get; set; }
        public string type { get; set; }
        public string paint_color { get; set; }
        public string state { get; set; }
        public DateTime posting_date { get; set; }
    }
}
