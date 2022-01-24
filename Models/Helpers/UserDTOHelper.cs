namespace Models.Helpers
{
    public static class UserDTOHelper
    {
        public static UserDTO ToDTO(User user)
        {
            return new UserDTO
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Zipcode = user.Zipcode,
            };
        }
    }
}