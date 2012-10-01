namespace Thinktecture.Wscf.Framework.Tests.Helpers
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.thinktecture.com", ConfigurationName="IRestaurantService")]
    public interface IRestaurantService
    {
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:thinktecture-com:demos:restaurantservice:messages:v1) of message getRestaurantsRequest does not match the default value (http://www.thinktecture.com)
        [System.ServiceModel.OperationContractAttribute(Action="http://www.thinktecture.com:getRestaurantsIn", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        GetRestaurantsResponse GetRestaurants(GetRestaurantsRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:thinktecture-com:demos:restaurantservice:messages:v1) of message addRestaurant does not match the default value (http://www.thinktecture.com)
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://www.thinktecture.com:addRestaurantIn")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void AddRestaurant(AddRestaurant request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:thinktecture-com:demos:restaurantservice:messages:v1) of message rateRestaurant does not match the default value (http://www.thinktecture.com)
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://www.thinktecture.com:rateRestaurantIn")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void RateRestaurant(RateRestaurant request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:data:v1", TypeName="restaurantInfo")]
    public partial class RestaurantInfo
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="restaurantID")]
        public int RestaurantID;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="name")]
        public string Name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="address")]
        public string Address;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="city")]
        public string City;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="state")]
        public string State;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="zip")]
        public string Zip;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="time", ElementName="openFrom")]
        public System.DateTime OpenFrom;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="time", ElementName="openTo")]
        public System.DateTime OpenTo;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(ElementName="choice")]
        public MyChoice Choice;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer", AttributeName="someattribute1")]
        public string Someattribute1;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="someattribute2")]
        public string Someattribute2;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:data:v1", TypeName="MyChoice")]
    public partial class MyChoice
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(FooCollection))]
        [System.Xml.Serialization.XmlElementAttribute("MoreData1", typeof(string))]
        public object Item;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:thinktecture-com:demos:restaurantservice:data:v1", TypeName="FooCollection")]
    public partial class FooCollection
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FooItem")]
        public string[] FooItem;
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getRestaurantsRequest", WrapperNamespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", IsWrapped=true)]
    public partial class GetRestaurantsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", Order=0, Name="zip")]
        public string Zip;
        
        public GetRestaurantsRequest()
        {
        }
        
        public GetRestaurantsRequest(string zip)
        {
            this.Zip = zip;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getRestaurantsResponse", WrapperNamespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", IsWrapped=true)]
    public partial class GetRestaurantsResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", Order=0, Name="restaurants")]
        [System.Xml.Serialization.XmlArrayItemAttribute("restaurant", Namespace="urn:thinktecture-com:demos:restaurantservice:data:v1", IsNullable=false)]
        public RestaurantInfo[] Restaurants;
        
        public GetRestaurantsResponse()
        {
        }
        
        public GetRestaurantsResponse(RestaurantInfo[] restaurants)
        {
            this.Restaurants = restaurants;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="addRestaurant", WrapperNamespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", IsWrapped=true)]
    public partial class AddRestaurant
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", Order=0, Name="restaurant")]
        public RestaurantInfo Restaurant;
        
        public AddRestaurant()
        {
        }
        
        public AddRestaurant(RestaurantInfo restaurant)
        {
            this.Restaurant = restaurant;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:data:v1", TypeName="ratingInfo")]
    public enum RatingInfo
    {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(Name="poor")]
        Poor,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(Name="good")]
        Good,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(Name="veryGood")]
        VeryGood,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(Name="excellent")]
        Excellent,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="rateRestaurant", WrapperNamespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", IsWrapped=true)]
    public partial class RateRestaurant
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", Order=0, Name="restaurantID")]
        public int RestaurantID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:thinktecture-com:demos:restaurantservice:messages:v1", Order=1, Name="rate")]
        public RatingInfo Rate;
        
        public RateRestaurant()
        {
        }
        
        public RateRestaurant(int restaurantID, RatingInfo rate)
        {
            this.RestaurantID = restaurantID;
            this.Rate = rate;
        }
    }
    
    [System.ServiceModel.ServiceBehaviorAttribute(InstanceContextMode=System.ServiceModel.InstanceContextMode.PerCall, ConcurrencyMode=System.ServiceModel.ConcurrencyMode.Single, UseSynchronizationContext=false)]
    public class RestaurantService : IRestaurantService
    {
        
        public virtual GetRestaurantsResponse GetRestaurants(GetRestaurantsRequest request)
        {
            throw new System.NotImplementedException();
        }
        
        public virtual void AddRestaurant(AddRestaurant request)
        {
            throw new System.NotImplementedException();
        }
        
        public virtual void RateRestaurant(RateRestaurant request)
        {
            throw new System.NotImplementedException();
        }
    }
}
