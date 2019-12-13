/// <summary>
/// MIT License
/// 
/// Copyright(c) 2019 Jeffrey Lewis
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
/// </summary>
using Stratis.SmartContracts;
using System;

[Deploy]
public class AnnoStratisContract : SmartContract
{
    /// <summary>
    /// Constructor used to create a new instance of the token. Assigns the total token supply to the creator of the contract.
    /// </summary>
    /// <param name="smartContractState">The execution state for the contract.</param>
    /// <param name="totalSupply">The total token supply.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="symbol">The symbol used to identify the token.</param>
    //public AnnoStratisContract(ISmartContractState smartContractState, ulong totalSupply, string name, string symbol)
    //        : base(smartContractState)
    //{
    //    this.TotalSupply = totalSupply;
    //    this.Name = name;
    //    this.Symbol = symbol;
    //    this.SetBalance(Message.Sender, totalSupply);
    //}
    public AnnoStratisContract(ISmartContractState smartContractState, ulong firstBlockTime)
        : base(smartContractState)
    {
        this.TotalSupply = 100_000_000;
        this.Name = "ANNO";
        this.Symbol = "ANNS";
        this.SetBalance(Message.Sender, 100_000_000);
        this.FirstBlockTime = firstBlockTime;
    }

    #region Persistence

    public string Symbol
    {
        get { return PersistentState.GetString(nameof(this.Symbol)); }
        private set { PersistentState.SetString(nameof(this.Symbol), value); }
    }

    public string Name
    {
        get { return PersistentState.GetString(nameof(this.Name)); }
        private set { PersistentState.SetString(nameof(this.Name), value); }
    }

    public string Owner
    {
        get { return PersistentState.GetString(nameof(this.Owner)); }
        private set { PersistentState.SetString(nameof(this.Owner), value); }
    }
    
    public ulong TotalSupply
    {
        get { return PersistentState.GetUInt64(nameof(this.TotalSupply)); }
        private set { PersistentState.SetUInt64(nameof(this.TotalSupply), value); }
    }

    public ulong FirstBlockTime
    {
        get { return PersistentState.GetUInt64(nameof(this.FirstBlockTime)); }
        private set { PersistentState.SetUInt64(nameof(this.FirstBlockTime), value); }
    }

    public ulong GetBalance(Address address)
    {
        return PersistentState.GetUInt64($"Balance:{address}");
    }
    private void SetBalance(Address address, ulong value)
    {
        PersistentState.SetUInt64($"Balance:{address}", value);
    }

    public ulong GetBalance(string address)
    {
        return PersistentState.GetUInt64($"Balance:{address}");
    }
    private void SetBalance(string address, ulong value)
    {
        PersistentState.SetUInt64($"Balance:{address}", value);
    }

    private string GetHost(Address address)
    {
        return PersistentState.GetString($"Host:{address}");
    }
    private void SetHost(Address address, string value)
    {
        PersistentState.SetString($"Host:{address}", value);
    }

    private string GetCustomer(Address address)
    {
        return PersistentState.GetString($"Customer:{address}");
    }
    private void SetCustomer(Address address, string value)
    {
        PersistentState.SetString($"Customer:{address}", value);
    }

    private string GetEvent(string eventUniqueId)
    {
        return PersistentState.GetString($"Event:{eventUniqueId}");
    }
    private void SetEvent(string eventUniqueId, string value)
    {
        PersistentState.SetString($"Event:{eventUniqueId}", value);
    }

    private string GetEventTier(string eventTierUniqueId)
    {
        return PersistentState.GetString($"EventTier:{eventTierUniqueId}");
    }
    private void SetEventTier(string eventTierUniqueId, string value)
    {
        PersistentState.SetString($"EventTier:{eventTierUniqueId}", value);
    }

    public ulong GetEventEscrow(string eventUniqueId)
    {
        return PersistentState.GetUInt64($"EventEscrow:{eventUniqueId}");
    }
    private void SetEventEscrow(string eventUniqueId, ulong value)
    {
        PersistentState.SetUInt64($"EventEscrow:{eventUniqueId}", value);
    }

    private string GetTicket(string ticketUniqueId)
    {
        return PersistentState.GetString($"Ticket:{ticketUniqueId}");
    }
    private void SetTicket(string ticketUniqueId, string value)
    {
        PersistentState.SetString($"Ticket:{ticketUniqueId}", value);
    }
    
    #endregion
    

    #region Public DApp Methods

    /// <summary>
    /// Transfer native currency from execution sender address to another address.
    /// </summary>
    public bool TransferTo(Address to, ulong amount)
    {
        if (amount == 0)
        {
            Log(new TransferLog { From = Message.Sender, To = to, Amount = 0 });
            return true;
        }

        ulong senderBalance = GetBalance(Message.Sender);
        if (senderBalance < amount)
        {
            Log(new GenericLog { Message = "TransferTo: Sender has insufficient balance." });
            return false;
        }

        SetBalance(Message.Sender, senderBalance - amount);
        SetBalance(to, checked(GetBalance(to) + amount));

        Log(new TransferLog { From = Message.Sender, To = to, Amount = amount });

        return true;
    }

    /// <summary>
    /// Transfer native currency from one address to another.
    /// </summary>
    public bool TransferFrom(Address from, Address to, ulong amount)
    {
        if (amount == 0)
        {
            Log(new TransferLog { From = from, To = to, Amount = 0 });
            return true;
        }
        
        ulong fromBalance = GetBalance(from);
        if (fromBalance < amount)
        {
            Log(new GenericLog { Message = "TransferFrom: Sender has insufficient balance." });
            return false;
        }
        
        SetBalance(from, fromBalance - amount);
        SetBalance(to, checked(GetBalance(to) + amount));

        Log(new TransferLog { From = from, To = to, Amount = amount });

        return true;
    }

    /// <summary>
    /// Creates a host. 
    /// A host is a person/organization who can create events for their customers to make bookings on.
    /// </summary>
    public bool CreateHost(Address hostAddress, string hostName)
    {
        //Validate input
        if (hostAddress == null || hostName == null)
        {
            Log(new GenericLog() { Method = "CreateHost", Message = "One or more required parameter is not specified." });
            return false;
        }

        //Put host data in storage
        string data = $"hostName:{hostName};";
        SetHost(hostAddress, data);

        //Init host account balance
        SetBalance(hostAddress, 0);

        Log(new GenericLog() { Method = "CreateHost", Message = $"Successfully created host: {hostName}." });

        return true;
    }

    /// <summary>
    /// Creates a customer.
    /// A customer is a user on the host's website or app. Customers are people who would book tickets to events created by hosts.
    /// </summary>
    public bool CreateCustomer(Address customerAddress, Address hostAddress, string customerRefId)
    {
        //Validate input
        if (customerAddress == null || hostAddress == null || customerRefId == null)
        {
            Log(new GenericLog() { Method = "CreateCustomer", Message = "One or more required parameter is not specified." });
            return false;
        }

        //Put customer data in storage
        string data = $"hostAddress:{hostAddress};customerRefId:{customerRefId};";
        SetCustomer(customerAddress, data);
        
        //Init customer account balance
        SetBalance(customerAddress, 0);

        Log(new GenericLog() { Method = "CreateCustomer", Message = $"Successfully created customer: {customerRefId}." });

        return true;
    }

    /// <summary>
    /// Creates an event.
    /// Events are created by host and put up for bookings. Customers of the host can make bookings on events.
    /// </summary>
    public bool CreateEvent(string eventUniqueId, Address hostAddress, string eventRefId, string title, ulong startDateTime, string status)
    {
        //Validate input
        if (eventUniqueId == null ||
            hostAddress == null ||
            eventRefId == null ||
            title == null ||
            status == null)
        {
            Log(new GenericLog() { Method = "CreateEvent", Message = "One or more required parameter is not specified." });
            return false;
        }

        //Put event data in storage
        string data = $"hostAddress:{hostAddress};eventRefId:{eventRefId};title:{title};startDateTime:{startDateTime};status:{status};";
        SetEvent(eventUniqueId, data);

        //Init event escrow balance
        //Note: 
        //Each event has it's own escrow for the purpose of collecting and refunding funds during bookings/cancellations.
        //Funds in the event's escrow are temporary held until the event start date is over, after which the host can claim all 
        //earnings via the ClaimEarnings operation. If the host cancels the event before the start date, the escrow is released back to customers.
        SetEventEscrow(eventUniqueId, 0);
        
        Log(new GenericLog() { Method = "CreateEvent", Message = $"Successfully created event: {title}." });

        return true;
    }

    /// <summary>
    /// Creates an event tier.
    /// A tier is a subcategory of an event with different pricing structure and availability.
    /// Example: A flight ticket would have the following tiers - Business Class, Preferred Seats, Economy
    /// </summary>
    public bool CreateEventTier(
        string eventTierUniqueId, 
        Address hostAddress, 
        string eventUniqueId, 
        string eventTierRefId, 
        string tierName, 
        uint totalTickets, 
        uint availableTickets, 
        ulong price)
    {
        //Validate input
        if (eventTierUniqueId == null ||
            hostAddress == null ||
            eventUniqueId == null ||
            eventTierRefId == null ||
            tierName == null)
        {
            Log(new GenericLog() { Method = "CreateEvent", Message = "One or more required parameter is not specified." });
            return false;
        }

        //Put tier data in storage
        string data = $"hostAddress:{hostAddress};eventUniqueId:{eventUniqueId};tierName:{tierName};totalTickets:{totalTickets};availableTickets:{availableTickets};price:{price};";
        SetEventTier(eventTierUniqueId, data);
        
        Log(new GenericLog() { Method = "CreateEventTier", Message = $"Successfully created event tier: {tierName}." });

        return true;
    }

    /// <summary>
    /// Book an event for a customer and generates the tickets based on the given booking details.
    /// </summary>
    public bool BookEvent(Address customerAddress, string eventUniqueId, string eventTierUniqueId, string ticketNumbersString)
    {
        //Validate input
        if (customerAddress == null || eventUniqueId == null || eventTierUniqueId == null || ticketNumbersString == null)
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "One or more required parameter is not specified." });
            return false;
        }

        //Get ticket numbers array from parameter
        string[] ticketNumbers = Split(ticketNumbersString, ';');
        
        //Validate booking ticket quantity
        if (ticketNumbers.Length > 1  && string.IsNullOrEmpty(ticketNumbers[0]))
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "Unable to book. Invalid ticket quantiy." });
            return false;
        }

        //Get event
        string eventData = GetEvent(eventUniqueId);
        if (string.IsNullOrEmpty(eventData))
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "Unable to book. Event not found." });
            return false;
        }

        //Get event tier
        string eventTierData = GetEventTier(eventTierUniqueId);
        if (string.IsNullOrEmpty(eventTierData))
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "Unable to book. Event tier not found." });
            return false;
        }

        //Validate event status
        if (GetDataValue(eventData, "status") != "Active")
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "Unable to book. Event is not active." });
            return false;
        }

        //Validate if event has already started
        if (CurrentTimestamp() > ulong.Parse(GetDataValue(eventData, "startDateTime")))
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "Unable to book. Event has already started." });
            return false;
        }

        //Validate if tickets are still available for the requested quantities
        if (int.Parse(GetDataValue(eventTierData, "availableTickets")) < ticketNumbers.Length)
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "Unable to book. Insufficient available tickets for the requested quantity." });
            return false;
        }

        //Calculate total booking cost
        ulong pricePerTicket = ulong.Parse(GetDataValue(eventTierData, "price"));
        ulong totalCost = pricePerTicket * (ulong)ticketNumbers.Length;

        //Check customer balance
        ulong customerBalance = GetBalance(customerAddress);
        if (customerBalance < totalCost)
        {
            Log(new GenericLog() { Method = "BookEvent", Message = "Unable to book. Customer has insufficient funds." });
            return false;
        }

        //Update available tickets in storage
        uint remainingAvailableTickets = uint.Parse(GetDataValue(eventTierData, "availableTickets")) - (uint)ticketNumbers.Length;
        eventTierData = SetDataValue(eventTierData, "availableTickets", remainingAvailableTickets.ToString());
        SetEventTier(eventTierUniqueId, eventTierData);

        //Transfer funds from customer account to event account
        TransferToEscrow(eventUniqueId, customerAddress, totalCost);

        //Store ticket data in persistence
        for (int i = 0; i < ticketNumbers.Length; i++)
        {
            string ticketData = $"customerAddress:{customerAddress};eventUniqueId:{eventUniqueId};eventTierUniqueId:{eventTierUniqueId};paidPrice:{pricePerTicket};status:Active;";
            SetTicket(ticketNumbers[i], ticketData);
        }
        
        Log(new GenericLog() { Method = "BookEvent", Message = "Booking successful." });

        return true;
    }

    /// <summary>
    /// Temporary Alternative to the BookEvent method.
    /// This one accepts a single ticket number instead of a list in order to reduce Gas usage and avoid exceeding Gas limits.
    /// Multi tickets bookings must invoke this method several times.
    /// </summary>
    public bool BookEventV2(Address customerAddress, string eventUniqueId, string eventTierUniqueId, string ticketNumber)
    {
        /*
         NOTE: 
         Certain functionality has been commented out to limit gas usage in order to 
         allow execution under default GasLimit of 100000 for Hackathon.
         These has to be restored or optimized for gas usage.
         */

        ////Validate input
        //if (customerAddress == null || eventUniqueId == null || eventTierUniqueId == null || ticketNumber == null)
        //{
        //    Log(new GenericLog() { Method = "BookEventV2", Message = "One or more required parameter is not specified." });
        //    return false;
        //}

        //Get event
        string eventData = GetEvent(eventUniqueId);
        if (string.IsNullOrEmpty(eventData))
        {
            Log(new GenericLog() { Method = "BookEventV2", Message = "Unable to book. Event not found." });
            return false;
        }

        //Get event tier
        string eventTierData = GetEventTier(eventTierUniqueId);
        if (string.IsNullOrEmpty(eventTierData))
        {
            Log(new GenericLog() { Method = "BookEventV2", Message = "Unable to book. Event tier not found." });
            return false;
        }

        ////Validate event status
        //if (GetDataValue(eventData, "status") != "Active")
        //{
        //    Log(new GenericLog() { Method = "BookEventV2", Message = "Unable to book. Event is not active." });
        //    return false;
        //}

        ////Validate if event has already started
        //if (CurrentTimestamp() > ulong.Parse(GetDataValue(eventData, "startDateTime")))
        //{
        //    Log(new GenericLog() { Method = "BookEventV2", Message = "Unable to book. Event has already started." });
        //    return false;
        //}

        ////Validate if tickets are still available for the requested quantities
        //uint availableTickets = uint.Parse(GetDataValue(eventTierData, "availableTickets"));
        //if (availableTickets < 1)
        //{
        //    Log(new GenericLog() { Method = "BookEventV2", Message = "Unable to book. Insufficient available tickets for the requested quantity." });
        //    return false;
        //}

        //Get booking cost
        ulong pricePerTicket = ulong.Parse(GetDataValue(eventTierData, "price"));

        //Check customer balance
        ulong customerBalance = GetBalance(customerAddress);
        if (customerBalance < pricePerTicket)
        {
            Log(new GenericLog() { Method = "BookEventV2", Message = $"Unable to book. Customer has insufficient funds." });
            return false;
        }

        ////Update available tickets in storage
        //eventTierData = SetDataValue(eventTierData, "availableTickets", (availableTickets - 1).ToString());
        //SetEventTier(eventTierUniqueId, eventTierData);

        //Transfer funds from customer account to event account
        TransferToEscrow(eventUniqueId, customerAddress, pricePerTicket);

        string ticketData = $"customerAddress:{customerAddress};eventUniqueId:{eventUniqueId};eventTierUniqueId:{eventTierUniqueId};paidPrice:{pricePerTicket};status:Active;";
        SetTicket(ticketNumber, ticketData);

        Log(new GenericLog() { Method = "BookEventV2", Message = "Booking successful." });

        return true;
    }

    /// <summary>
    /// Validates and redeems a ticket. Used tickets cannot be refunded.
    /// </summary>
    public bool RedeemTicket(string ticketNumber)
    {
        //Get ticket
        string ticketData = GetTicket(ticketNumber);
        if (string.IsNullOrEmpty(ticketData))
        {
            Log(new GenericLog() { Method = "RedeemTicket", Message = "Unable to redeem. Ticket not found." });
            return false;
        }

        //Check if ticket is still active
        if (GetDataValue(ticketData, "status") != "Active")
        {
            Log(new GenericLog() { Method = "RedeemTicket", Message = "Unable to redeem. Ticket is no longer active." });
            return false;
        }

        //Update ticket status
        string updatedData = SetDataValue(ticketData, "status", "Used");
        SetTicket(ticketNumber, updatedData);

        return true;
    }

    /// <summary>
    /// Cancels a ticket and refund the paid price from the event account to the customer account.
    /// </summary>
    public bool CancelTicket(string ticketNumber)
    {
        //Get ticket
        string ticketData = GetTicket(ticketNumber);
        if (string.IsNullOrEmpty(ticketData))
        {
            Log(new GenericLog() { Method = "CancelTicket", Message = "Unable to cancel. Ticket not found." });
            return false;
        }

        //Get event
        string eventUniqueId = GetDataValue(ticketData, "eventUniqueId");
        string eventData = GetEvent(eventUniqueId);
        if (string.IsNullOrEmpty(eventData))
        {
            Log(new GenericLog() { Method = "CancelTicket", Message = "Unable to cancel. Event not found." });
            return false;
        }

        //Get event tier
        string eventTierUniqueId = GetDataValue(ticketData, "eventTierUniqueId");
        string eventTierData = GetEventTier(eventTierUniqueId);
        if (string.IsNullOrEmpty(eventTierData))
        {
            Log(new GenericLog() { Method = "CancelTicket", Message = "Unable to cancel. Event tier not found." });
            return false;
        }

        //Check if ticket is still active
        if (GetDataValue(ticketData, "status") == "Used" || GetDataValue(ticketData, "status") == "Cancelled")
        {
            Log(new GenericLog() { Method = "CancelTicket", Message = "Unable to cancel. Ticket has been used or cancelled." });
            return false;
        }

        //Update ticket status in storage
        string updatedData = SetDataValue(ticketData, "status", "Cancelled");
        SetTicket(ticketNumber, updatedData);

        //Refund ticket price to customer by transferring funds from event account to customer account
        string customerAddress = GetDataValue(ticketData, "customerAddress");
        ulong ticketPrice = ulong.Parse(GetDataValue(ticketData, "paidPrice"));
        TransferFromEscrow(eventUniqueId, customerAddress, ticketPrice);

        //Update available tickets in storage
        uint newAvailableTickets = uint.Parse(GetDataValue(eventTierData, "availableTickets")) + 1;
        eventTierData = SetDataValue(eventTierData, "availableTickets", newAvailableTickets.ToString());
        SetEventTier(eventTierUniqueId, eventTierData);

        Log(new GenericLog() { Method = "CancelTicket", Message = "Cancellation successful." });

        return true;
    }

    /// <summary>
    /// Claims the earnings of an event after the event start date is over.
    /// All funds from the event account is transferred to the host account.
    /// </summary>
    public bool ClaimEarnings(string eventUniqueId)
    {
        //Get event
        string eventData = GetEvent(eventUniqueId);
        if (string.IsNullOrEmpty(eventData))
        {
            Log(new GenericLog() { Method = "ClaimEarnings", Message = "Event not found." });
            return false;
        }

        //Check if event status is not closed or cancelled
        if (GetDataValue(eventData, "status") == "Closed" || GetDataValue(eventData, "status") == "Cancelled")
        {
            Log(new GenericLog() { Method = "ClaimEarnings", Message = "Event is already claimed or cancelled." });
            return false;
        }

        //Transfer all funds from event escrow to host account
        string hostAddress = GetDataValue(eventData, "hostAddress");
        TransferFromEscrow(eventUniqueId, hostAddress, GetEventEscrow(eventUniqueId));

        //Update event status in storage
        string updatedData = SetDataValue(eventData, "status", "Closed");
        SetEvent(eventUniqueId, updatedData);

        return true;
    }

    #endregion

    #region Private DApp Methods

    /// <summary>
    /// Transfer native currency from an address to an event's escrow.
    /// </summary>
    private bool TransferToEscrow(string eventUniqueId, Address fromAddress, ulong amount)
    {
        if (amount == 0)
        {
            return true;
        }

        ulong fromBalance = GetBalance(fromAddress);
        if (fromBalance < amount)
        {
            Log(new GenericLog { Message = "TransferToEscrow: Sender has insufficient balance." });
            return false;
        }

        SetBalance(fromAddress, fromBalance - amount);
        SetEventEscrow(eventUniqueId, checked(GetEventEscrow(eventUniqueId) + amount));

        return true;
    }

    /// <summary>
    /// Transfer native currency an event's escrow to an address
    /// </summary>
    private bool TransferFromEscrow(string eventUniqueId, string toAddress, ulong amount)
    {
        if (amount == 0)
        {
            return true;
        }

        ulong escrowBalance = GetEventEscrow(eventUniqueId);
        if (escrowBalance < amount)
        {
            Log(new GenericLog { Message = "TransferFromEscrow: Escrow has insufficient balance." });
            return false;
        }

        SetEventEscrow(eventUniqueId, escrowBalance - amount);
        SetBalance(toAddress, checked(GetBalance(toAddress) + amount));

        return true;
    }

    #endregion


    #region Contract Helpers

    /// <summary>
    /// Gets the current time.
    /// </summary>
    /// <returns>Current uint format date.</returns>
    private ulong CurrentTimestamp()
    {
        return this.Block.Number * 16 + this.FirstBlockTime;
    }

    #endregion

    #region Collection Helpers

    // The following methods are substitute for the System.Collections library as it is not supported.
    // The purpose of using collections is to allow storing a KeyValue collection in a single storage key, 
    // allowing for structured data in storage.

    private string GetDataValue(string data, string key)
    {
        bool found = false;
        string result = null;

        string[] keys = GetDataKeys(data);
        string[] values = GetDataValues(data);
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == null)
            {
                break;
            }
            if (keys[i] == key)
            {
                result = values[i];
                found = true;
                break;
            }
        }
        if (found)
        {
            return result;
        }
        else
        {
            return string.Empty;
        }
    }
    private string SetDataValue(string data, string key, string value)
    {
        string[] keys = GetDataKeys(data);
        string[] values = GetDataValues(data);
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == key)
            {
                values[i] = value;
                break;
            }
        }

        //serialize to {key1}:{value1};{key2}:{value2};...
        string result = "";

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == null)
            {
                break;
            }

            result += keys[i] + ":" + values[i];

            if (i < keys.Length - 1)
            {
                result += ";";
            }
        }

        return result;
    }

    private string[] GetDataKeys(string data)
    {
        string[] result = new string[100];
        int resultCount = 0;

        string[] items = Split(data, ';');
        for (int i = 0; i < items.Length; i++)
        {
            string[] t = Split(items[i], ':');
            result[resultCount] = t.Length > 0 ? t[0] : null;
            resultCount++;
        }

        return result;
    }
    private string[] GetDataValues(string data)
    {
        string[] result = new string[100];
        int resultCount = 0;

        string[] items = Split(data, ';');
        for (int i = 0; i < items.Length; i++)
        {
            string[] t = Split(items[i], ':');
            result[resultCount] = t.Length > 1 ? t[1] : null;
            resultCount++;
        }

        return result;
    }

    /// <summary>
    /// Substitute for the string.Split method as string.Split is not supported.
    /// </summary>
    public static string[] Split(string input, char delimiter)
    {
        string[] bufferArray = new string[100];
        string buffer = "";
        int bufferArrayCount = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == delimiter)
            {
                bufferArray[bufferArrayCount] = buffer;
                bufferArrayCount++;
                buffer = "";
            }
            else
            {
                buffer += input[i];
            }
        }
        bufferArray[bufferArrayCount] = buffer;
        bufferArrayCount++;

        //resize array
        int length = 0;
        for (int i = 0; i < bufferArray.Length; i++)
        {
            if (bufferArray[i] == null)
            {
                break;
            }
            else
            {
                length++;
            }
        }
        string[] result = new string[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = bufferArray[i];
        }

        return result;
    }

    #endregion

    #region Struts

    public struct GenericLog
    {
        public string Method;

        public string Message;
    }

    public struct TransferLog
    {
        [Index]
        public Address From;

        [Index]
        public Address To;

        public ulong Amount;
    }

    #endregion
}