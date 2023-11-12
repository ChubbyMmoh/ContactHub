using ContactApi.Model;

namespace ContactApi.Data.Repository.Interface
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync(int page, int pageSize);
        Task<Contact> GetContactByIdAsync(Guid id);
        Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm, int page, int pageSize);
        Task<Contact> AddContactAsync(AddContactRequest contact);
        Task<Contact> UpdateContactAsync(Guid id, UpdateContactRequest contact);
        Task<bool> DeleteContactAsync(Guid id);
        Task UpdatePhotoAsync(Guid id, string imageUrl);
    }

}
