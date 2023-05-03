using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesDatabaseApp
{
    public class InstructorCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public int InstructorId { get; set; }

        [NotMapped]
        public virtual Instructor Instructor { get; set; }

        public int CourseId { get; set; }

        [NotMapped]
        public virtual Course Course { get; set; }
    }

}
