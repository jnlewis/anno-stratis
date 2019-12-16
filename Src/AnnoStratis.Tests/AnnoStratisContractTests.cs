namespace AnnoStratis.Tests
{
    using Moq;
    using Stratis.SmartContracts;
    using Stratis.SmartContracts.CLR;
    using Xunit;

    public class AnnoStratisContractTests
    {
        private readonly Mock<ISmartContractState> mockContractState;
        private readonly Mock<IPersistentState> mockPersistentState;
        private readonly Mock<IContractLogger> mockContractLogger;
        private Address owner;
        private Address sender;
        private Address contract;
        private Address destination;
        private ulong firstBlockTime;
        private string name;
        private string symbol;

        public AnnoStratisContractTests()
        {
            this.mockContractLogger = new Mock<IContractLogger>();
            this.mockPersistentState = new Mock<IPersistentState>();
            this.mockContractState = new Mock<ISmartContractState>();
            this.mockContractState.Setup(s => s.PersistentState).Returns(this.mockPersistentState.Object);
            this.mockContractState.Setup(s => s.ContractLogger).Returns(this.mockContractLogger.Object);
            this.owner = "0x0000000000000000000000000000000000000001".HexToAddress();
            this.sender = "0x0000000000000000000000000000000000000002".HexToAddress();
            this.contract = "0x0000000000000000000000000000000000000003".HexToAddress();
            this.destination = "0x0000000000000000000000000000000000000005".HexToAddress();

            this.firstBlockTime = 1572566400;
            this.name = "ANNO";
            this.symbol = "ANNS";
        }

        [Fact]
        public void Constructor_Sets_FirstBlockTime()
        {
            // Set the message that would act as the initial transaction to the contract.
            this.mockContractState.Setup(m => m.Message).Returns(new Message(this.contract, this.owner, 0));

            // Create a new instance of the smart contract class.
            var contract = new AnnoStratisContract(this.mockContractState.Object, this.firstBlockTime);

            // Verify that PersistentState was called with the first block time
            this.mockPersistentState.Verify(s => s.SetUInt64(nameof(contract.FirstBlockTime), this.firstBlockTime));
        }

        [Fact]
        public void Constructor_Sets_Name_And_Symbol()
        {
            // Set the message that would act as the initial transaction to the contract.
            this.mockContractState.Setup(m => m.Message).Returns(new Message(this.contract, this.owner, 0));

            // Create a new instance of the smart contract class.
            var contract = new AnnoStratisContract(this.mockContractState.Object, this.firstBlockTime);

            // Verify we set the name and the symbol
            this.mockPersistentState.Verify(s => s.SetString("Name", this.name));
            this.mockPersistentState.Verify(s => s.SetString("Symbol", this.symbol));
        }

        [Fact]
        public void Constructor_Sets_TotalSupply()
        {
            this.mockContractState.Setup(m => m.Message).Returns(new Message(this.contract, this.owner, 0));

            ulong totalSupply = 100_000_000;
            var contract = new AnnoStratisContract(this.mockContractState.Object, this.firstBlockTime);

            // Verify that PersistentState was called with the total supply
            this.mockPersistentState.Verify(s => s.SetUInt64(nameof(contract.TotalSupply), totalSupply));
        }

        [Fact]
        public void TransferFrom_Greater_Than_Owners_Balance_Returns_False()
        {
            ulong firstBlockTime = 1572566400;
            ulong balance = 0; // Balance should be less than amount we are trying to send
            ulong amount = balance + 1;

            // Setup the Message.Sender address
            this.mockContractState.Setup(m => m.Message)
                .Returns(new Message(this.contract, this.sender, 0));

            var contract = new AnnoStratisContract(this.mockContractState.Object, firstBlockTime);

            // Setup the balance of the owner in persistent state
            this.mockPersistentState.Setup(s => s.GetUInt64($"Balance:{this.owner}")).Returns(balance);

            Assert.False(contract.TransferFrom(this.owner, this.destination, amount));

            // Verify we queried the owner's balance
            this.mockPersistentState.Verify(s => s.GetUInt64($"Balance:{this.owner}"));
        }

        [Fact]
        public void CreateHost_Sets_Host_And_Returns_True()
        {
            string hostName = "Airbnb";
            Address hostAddress = "0x0000000000000000000000000000000000000007".HexToAddress();

            string data = $"hostName:{hostName};";

            this.mockContractState.Setup(m => m.Message).Returns(new Message(this.contract, this.owner, 0));

            var contract = new AnnoStratisContract(this.mockContractState.Object, this.firstBlockTime);

            Assert.True(contract.CreateHost(hostAddress, hostName));

            this.mockPersistentState.Verify(s => s.SetString($"Host:{hostAddress}", data));
        }

        [Fact]
        public void CreateCustomer_Sets_Customer_And_Returns_True()
        {
            string customerRefId = "CI00001";
            Address hostAddress = "0x0000000000000000000000000000000000000007".HexToAddress();
            Address customerAddress = "0x0000000000000000000000000000000000000008".HexToAddress();

            string data = $"hostAddress:{hostAddress};customerRefId:{customerRefId};";

            this.mockContractState.Setup(m => m.Message).Returns(new Message(this.contract, this.owner, 0));

            var contract = new AnnoStratisContract(this.mockContractState.Object, this.firstBlockTime);

            Assert.True(contract.CreateCustomer(customerAddress, hostAddress, customerRefId));

            this.mockPersistentState.Verify(s => s.SetString($"Customer:{customerAddress}", data));
        }

        [Fact]
        public void CreateEvent_Sets_Event_And_Returns_True()
        {
            string eventUniqueId = "X1QBzO7byCWIOYHt";
            Address hostAddress = "0x0000000000000000000000000000000000000007".HexToAddress();
            string eventRefId = "EXP18001";
            string title = "Uppermost Concert Singapore";
            ulong startDateTime = 1576368000;
            string status = "Active";

            string data = $"hostAddress:{hostAddress};eventRefId:{eventRefId};title:{title};startDateTime:{startDateTime};status:{status};";

            this.mockContractState.Setup(m => m.Message).Returns(new Message(this.contract, this.owner, 0));

            var contract = new AnnoStratisContract(this.mockContractState.Object, this.firstBlockTime);

            Assert.True(contract.CreateEvent(eventUniqueId, hostAddress, eventRefId, title, startDateTime, status));

            this.mockPersistentState.Verify(s => s.SetString($"Event:{eventUniqueId}", data));
        }

        [Fact]
        public void CreateEventTier_Sets_EventTier_And_Returns_True()
        {
            string eventTierUniqueId = "sSng90YmnxrCbMAe";
            Address hostAddress = "0x0000000000000000000000000000000000000007".HexToAddress();
            string eventUniqueId = "X1QBzO7byCWIOYHt";
            string eventTierRefId = "EXP18001T01";
            string tierName = "Normal Admission";
            uint totalTickets = 50;
            uint availableTickets = 50;
            ulong price = 238;

            string data = $"hostAddress:{hostAddress};eventUniqueId:{eventUniqueId};tierName:{tierName};totalTickets:{totalTickets};availableTickets:{availableTickets};price:{price};";

            this.mockContractState.Setup(m => m.Message).Returns(new Message(this.contract, this.owner, 0));

            var contract = new AnnoStratisContract(this.mockContractState.Object, this.firstBlockTime);

            Assert.True(contract.CreateEventTier(eventTierUniqueId, hostAddress, eventUniqueId, eventTierRefId, tierName, totalTickets, availableTickets, price));

            this.mockPersistentState.Verify(s => s.SetString($"EventTier:{eventTierUniqueId}", data));
        }

        // TODO: Write tests for event booking validation, event escrow balance check, cancellation refunds
    }
}
