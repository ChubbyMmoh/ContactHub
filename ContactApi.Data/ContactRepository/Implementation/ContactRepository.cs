using ContactApi.Core.Interface;
using ContactApi.Data.DbContext;
using ContactApi.Data.Repository.Interface;
using ContactApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ContactApi.Data.Repository.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactApiDbContext _dbContext;
        private readonly ICloudinaryService _cloudinaryService;

        public ContactRepository(ContactApiDbContext dbContext, ICloudinaryService cloudinaryService)
        {
            _dbContext = dbContext;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync(int page, int pageSize)
        {
            return await _dbContext.Contacts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Contact> GetContactByIdAsync(Guid id)
        {
            return await _dbContext.Contacts.FindAsync(id);
        }

        public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm, int page, int pageSize)
        {
            
            return await _dbContext.Contacts
                .Where(contact => contact.FullName.Contains(searchTerm))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Contact> AddContactAsync(AddContactRequest contact)
        {
            var newContact = new Contact
            {
                Id = Guid.NewGuid(),
                FullName = contact.FullName,
                Email = contact.Email,
                Phone = contact.Phone,
                Address = contact.Address,
            };

            await _dbContext.Contacts.AddAsync(newContact);
            await _dbContext.SaveChangesAsync();

            return newContact;
        }

        public async Task<Contact> UpdateContactAsync(Guid id, UpdateContactRequest contact)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(id);

            if (existingContact != null)
            {
                existingContact.FullName = contact.FullName;
                existingContact.Email = contact.Email;
                existingContact.Phone = contact.Phone;
                existingContact.Address = contact.Address;

                await _dbContext.SaveChangesAsync();
            }


            return existingContact;
        }

        public async Task<bool> DeleteContactAsync(Guid id)
        {
            var contactToDelete = await _dbContext.Contacts.FindAsync(id);

            if (contactToDelete != null)
            {
                _dbContext.Contacts.Remove(contactToDelete);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task UpdatePhotoAsync(Guid id, string imageUrl)
        {
            var foundContact = await _dbContext.Contacts.FindAsync(id);
            if (foundContact != null)
            {
                foundContact.PhotoUrl = imageUrl; 

                _dbContext.Update(foundContact);
                await _dbContext.SaveChangesAsync();
            }
        }

    }

}


