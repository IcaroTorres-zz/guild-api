namespace Domain.Enums
{
    public enum InviteStatuses : short
    {
        /// <summary>
        /// Waiting for an answer as accepted, declined or canceled
        /// </summary>
        Pending = 1,
        /// <summary>
        /// Accepted by invited member
        /// </summary>
        Accepted,
        /// <summary>
        /// Declined by invited member
        /// </summary>
        Declined,
        /// <summary>
        /// Canceled by inviting guild
        /// </summary>
        Canceled
    }
}
