using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.Models
{
    public class SolutionComponentModel
    {
        public List<SolutionComponentModelValue> value { get; set; }
        /*public Guid _organizationid_value { get; set; }
        public int webresourcetype { get; set; }
        public DateTime createdon { get; set; }
        public string displayname { get; set; }
        public Guid solutionid { get; set; }

        public string content { get; set; }

        public Guid webresourceid { get; set; }

        public bool CanBeChanged { get; set; }

        public string name { get; set; }

        /// <summary>
        /// /////////////////////////////////////
        /// </summary>
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("_modifiedby_value")]
        public Guid ModifiedbyValue { get; set; }

        [JsonProperty("objectid")]
        public Guid Objectid { get; set; }

        [JsonProperty("componenttype")]
        public int Componenttype { get; set; }

        [JsonProperty("modifiedon")]
        public DateTime Modifiedon { get; set; }

        [JsonProperty("solutioncomponentid")]
        public Guid Solutioncomponentid { get; set; }

        [JsonProperty("createdon")]
        public DateTime Createdon { get; set; }

        //[JsonProperty("rootcomponentbehavior")]
        //public long Rootcomponentbehavior { get; set; }

        //[JsonProperty("versionnumber")]
        //public long Versionnumber { get; set; }

        //[JsonProperty("ismetadata")]
        //public bool Ismetadata { get; set; }

        [JsonProperty("_solutionid_value")]
        public Guid SolutionidValue { get; set; }

        //[JsonProperty("_createdby_value")]
        //public Guid CreatedbyValue { get; set; }

        //[JsonProperty("_modifiedonbehalfby_value")]
        //public Guid? ModifiedonbehalfbyValue { get; set; }

        //[JsonProperty("rootsolutioncomponentid")]
        //public object Rootsolutioncomponentid { get; set; }

        //[JsonProperty("_createdonbehalfby_value")]
        //public Guid? CreatedonbehalfbyValue { get; set; }*/
    }

    public class SolutionComponentModelValue
    {

        public Guid objectid { get; set; }
        public int componenttype { get; set; }
        public DateTime modifiedon { get; set; }
        public Guid solutioncomponentid { get; set; }
        public DateTime createdon { get; set; }

        public Guid solutionid { get; set; }

    }

   
    enum ComponentType
    {
        Entity = 1,
        Attribute = 2,
        Relationship = 3,
        Attribute_Picklist_Value = 4,
        Attribute_Lookup_Value = 5,
        View_Attribute = 6,
        Localized_Label = 7,
        Relationship_Extra_Condition = 8,
        Option_Set = 9,
        Entity_Relationship = 10,
        Entity_Relationship_Role = 11,
        Entity_Relationship_Relationships = 12,
        Managed_Property = 13,
        Entity_Key = 14,
        Privilege = 16,
        PrivilegeObjectTypeCode = 17,
        Index = 18,
        Role = 20,
        Role_Privilege = 21,
        Display_String = 22,
        Display_String_Map = 23,
        Form = 24,
        Organization = 25,
        Saved_Query = 26,
        Workflow = 29,
        Report = 31,
        Report_Entity = 32,
        Report_Category = 33,
        Report_Visibility = 34,
        Attachment = 35,
        Email_Template = 36,
        Contract_Template = 37,
        KB_Article_Template = 38,
        Mail_Merge_Template = 39,
        Duplicate_Rule = 44,
        Duplicate_Rule_Condition = 45,
        Entity_Map = 46,
        Attribute_Map = 47,
        Ribbon_Command = 48,
        Ribbon_Context_Group = 49,
        Ribbon_Customization = 50,
        Ribbon_Rule = 52,
        Ribbon_Tab_To_Command_Map = 53,
        Ribbon_Diff = 55,
        Saved_Query_Visualization = 59,
        System_Form = 60,
        Web_Resource = 61,
        Site_Map = 62,
        Connection_Role = 63,
        Complex_Control = 64,
        Hierarchy_Rule = 65,
        Custom_Control = 66,
        Custom_Control_Default_Config = 68,
        Field_Security_Profile = 70,
        Field_Permission = 71,
        Plugin_Type = 90,
        Plugin_Assembly = 91,
        SDK_Message_Processing_Step = 92,
        SDK_Message_Processing_Step_Image = 93,
        Service_Endpoint = 95,
        Routing_Rule = 150,
        Routing_Rule_Item = 151,
        SLA = 152,
        SLA_Item = 153,
        Convert_Rule = 154,
        Convert_Rule_Item = 155,
        Mobile_Offline_Profile = 161,
        Mobile_Offline_Profile_Item = 162,
        Similarity_Rule = 165,
        Data_Source_Mapping = 166,
        SDKMessage = 201,
        SDKMessageFilter = 202,
        SdkMessagePair = 203,
        SdkMessageRequest = 204,
        SdkMessageRequestField = 205,
        SdkMessageResponse = 206,
        SdkMessageResponseField = 207,
        Import_Map = 208,
        WebWizard = 210,
        Canvas_App = 300,
        Connector = 371,

    }
}
