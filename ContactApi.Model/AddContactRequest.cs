namespace ContactApi.Model
{
    public class AddContactRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PhotoUrl { get; set; }
    }
}
