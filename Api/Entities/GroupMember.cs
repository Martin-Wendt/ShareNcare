namespace Api.Entities
{
    public class GroupMember
    {
        public User User { get; }
        public List<GroupPayment> Payments { get; } = new();
        public GroupMember(User user)
        {
            User = user;
        }   

    }

}
