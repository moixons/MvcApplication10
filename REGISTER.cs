namespace MvcApplication10
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("REGISTER")]
    public partial class REGISTER
    {
        public int SESSIONID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDUSER { get; set; }

        public DateTime DLOGIN { get; set; }

        public DateTime? DLOGOUT { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        public bool? CONNECT { get; set; }
    }
}
