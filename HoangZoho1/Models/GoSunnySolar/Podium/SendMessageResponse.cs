using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.Podium
{

    public class SendMessageResponse
    {
        public MessageData data { get; set; }
        public MessageMetadata metadata { get; set; }
    }

    public class MessageData
    {

        public string attachmentUrl { get; set; }
        
        public string body { get; set; }
        
        public object contact { get; set; }
        
        public string contactName { get; set; }
        
        public MessageConversation conversation { get; set; }
        
        public DateTime? createdAt { get; set; }
        
        public string failureReason { get; set; }
        
        public MessageItem[] items { get; set; }
        
        public MessageLocation location { get; set; }
        
        public object sender { get; set; }
        
        public object senderUid { get; set; }
        
        public string uid { get; set; }

    }

    public class MessageConversation
    {

        public string assignedUserUid { get; set; }
        
        public MessageChannel channel { get; set; }
        
        public DateTime? startedAt { get; set; }
        
        public string uid { get; set; }
    
    }

    public class MessageChannel
    {

        public string identifier { get; set; }
        
        public string type { get; set; }
    
    }

    public class MessageLocation
    {

        public string organizationUid { get; set; }
        
        public string uid { get; set; }
    
    }

    public class MessageItem
    {

        public string attachmentContentType { get; set; }
        
        public string attachmentUrl { get; set; }
        
        public string body { get; set; }
        
        public string deliveryStatus { get; set; }
        
        public string sourceType { get; set; }
        
        public string type { get; set; }
        
        public string uid { get; set; }
        
        public object[] actions { get; set; }
        
        public string applicationUid { get; set; }
        
        public string externalId { get; set; }
        
        public object header { get; set; }
        
        public string iconUrl { get; set; }
        
        public string id { get; set; }
        
        public bool? isMessage { get; set; }
        
        public object[] listItems { get; set; }
        
        public object listViewLabel { get; set; }
        
        public object originatedFrom { get; set; }
        
        public object[] paymentLineItems { get; set; }
        
        public string publishDate { get; set; }
        
        public string resourceId { get; set; }
        
        public string senderName { get; set; }
        
        public object subheader { get; set; }
        
        public object subject { get; set; }
        
        public object[] timeSlots { get; set; }
        
        public MessageUser user { get; set; }
        
        public string userUid { get; set; }
    
    }

    public class MessageUser
    {

        public object avatarUrl { get; set; }

    }

    public class MessageMetadata
    {

        public string url { get; set; }
    
    }

}
