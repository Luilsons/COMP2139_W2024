using System.ComponentModel.DataAnnotations;

namespace MVC_Application.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        [Required]

        public string Name { get; set; }

        //nullable?
        public string? Description { get; set; }

        [DataType(DataType.Date)] 
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        //nullable?
        public string Status { get; set; }

        public List<ProjectTask>? Tasks { get; set;}
        

    }
}
