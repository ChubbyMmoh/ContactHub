﻿namespace ContactApi.Model.DTO
{
    public class AddContactRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PhotoUrl { get; set; }
    }
}
