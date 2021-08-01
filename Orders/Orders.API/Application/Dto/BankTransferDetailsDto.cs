namespace Orders.API.Application.Dto
{
    public class BankTransferDetailsDto
    {
        public string Title { get; init; }
        public string AccountNumber { get; init; }
        public decimal TransferAmount { get; init; }
    }
}
