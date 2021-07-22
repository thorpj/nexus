using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexus.Shared.Models.Autotask
{
    public class Ticket : BaseEntity
    {
        [JsonPropertyName("id")]
        public int? ID { get; set; }
        public int? ApiVendorID { get; set; }
        public int? AssignedResourceID { get; set; }
        public int? AssignedResourceRoleID { get; set; }
        public int? BillingCodeID { get; set; }
        public string? ChangeApprovalBoard { get; set; }
        public int? ChangeApprovalStatus { get; set; }
        public int? ChangeApprovalType { get; set; }
        public string? ChangeInfoField1 { get; set; }
        public string? ChangeInfoField2 { get; set; }
        public string? ChangeInfoField3 { get; set; }
        public string? ChangeInfoField4 { get; set; }
        public string? ChangeInfoField5 { get; set; }
        public int CompanyID { get; set; }
        public int? CompanyLocationID { get; set; }
        public int? CompletedByResourceID { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? ConfigurationItemID { get; set; }
        public int? ContactID { get; set; }
        public int? ContractID { get; set; }
        public int? ContractServiceBundleID { get; set; }
        public int? ContractServiceID { get; set; }
        public DateTime CreateDate { get; set; }
        public int? CreatedByContactID { get; set; }
        public int? CreatorResourceID { get; set; }
        public int? CreatorType { get; set; }
        public string? CurrentServiceThermometerRating { get; set; }
        public string Description { get; set; }
        public DateTime? DueDateTime { get; set; }
        public float EstimatedHours { get; set; }
        public string? ExternalID { get; set; }
        public int? FirstResponseAssignedResourceID { get; set; }
        public DateTime? FirstResponseDateTime { get; set; }
        public DateTime? FirstResponseDueDateTime { get; set; }
        public int? FirstResponseInitiatingResourceID { get; set; }
        public float? HoursToBeScheduled { get; set; }
        public int? ImpersonatorCreatorResourceID { get; set; }
        public int? IssueType { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public int? LastActivityPersonType { get; set; }
        public int? LastActivityResourceID { get; set; }
        public DateTime? LastCustomerNotificationDateTime { get; set; }
        public DateTime? LastCustomerVisibleActivityDateTime { get; set; }
        public DateTime? LastTrackedModificationDateTime { get; set; }
        public int? MonitorID { get; set; }
        public int? MonitorTypeID { get; set; }
        public int? OpportunityID { get; set; }
        public int? OrganizationalLevelAssociationID { get; set; }
        public int? PreviousServiceThermometerRating { get; set; }
        public int Priority { get; set; }
        public int? ProblemTicketId { get; set; }
        public int? ProjectID { get; set; }
        public string? PurchaseOrderNumber { get; set; }
        [JsonPropertyName("queueID")]
        public int? Queue { get; set; }
        public string? Resolution { get; set; }
        public DateTime? ResolutionPlanDateTime { get; set; }
        public DateTime? ResolutionPlanDueDateTime { get; set; }
        public DateTime? ResolvedDateTime { get; set; }
        public DateTime? ResolvedDueDateTime { get; set; }
        public string? RmaStatus { get; set; }
        public string? RmaType { get; set; }
        public int? RmmAlertID { get; set; }
        public bool? ServiceLevelAgreementHasBeenMet { get; set; }
        public int? ServiceLevelAgreementID { get; set; }
        public string? ServiceLevelAgreementPausedNextEventHours { get; set; }
        public string? ServiceThermometerTemperature { get; set; }
        public int? Source { get; set; }
        public Status Status { get; set; }
        public int? SubIssueType { get; set; }
        public int TicketCategory { get; set; }
        public string TicketNumber { get; set; }
        public int TicketType { get; set; }
        public string Title { get; set; }
        public List<UserDefinedField>? UserDefinedFields { get; set; }

        protected override string ImportantFieldsMessage() => $"id: {ID}, title: {Title}";

        public string TestFunc()
        {
            return $"ID: {ID}, Title: {Title}";
        }
        
    }

    public enum Status : int
    {
        [Description("New")]
        New = 1,
        [Description("Complete")]
        Complete = 5,
        [Description("Waiting Customer")]
        WaitingCustomer = 7,
        [Description("In Progress")]
        InProgress = 8,
        [Description("Pending Parts/Materials")]
        PendingPartsMaterials = 9,
        [Description("To Be Dispatched")]
        ToBeDispatched = 10,
        [Description("Pending Vendor")]
        PendingVendor = 12,
        [Description("Pending Approval")]
        PendingApproval = 13,
        [Description("Parts/Materials Received")]
        PartsMaterialsReceived = 14,
        [Description("Awaiting Device")]
        AwaitingDevice = 15,
        [Description("To Be Assessed")]
        ToBeAssessed = 16,
    }
    
 
}