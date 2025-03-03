namespace ResumeAPI.Models
{
    public class Education        
    { 
        public int ID { get; set; }
        public string School { get; set; }
        public string Field {  get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
