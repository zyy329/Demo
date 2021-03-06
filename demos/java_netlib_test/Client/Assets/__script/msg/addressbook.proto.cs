//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: proto/addressbook.proto
namespace tutorial
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Person")]
  public partial class Person : global::ProtoBuf.IExtensible
  {
    public Person() {}
    
    private string _name = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private string _email = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"email", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string email
    {
      get { return _email; }
      set { _email = value; }
    }
    private readonly global::System.Collections.Generic.List<tutorial.Person.PhoneNumber> _phones = new global::System.Collections.Generic.List<tutorial.Person.PhoneNumber>();
    [global::ProtoBuf.ProtoMember(4, Name=@"phones", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<tutorial.Person.PhoneNumber> phones
    {
      get { return _phones; }
    }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PhoneNumber")]
  public partial class PhoneNumber : global::ProtoBuf.IExtensible
  {
    public PhoneNumber() {}
    
    private string _number = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"number", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string number
    {
      get { return _number; }
      set { _number = value; }
    }
    private tutorial.Person.PhoneType _type = tutorial.Person.PhoneType.MOBILE;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(tutorial.Person.PhoneType.MOBILE)]
    public tutorial.Person.PhoneType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"PhoneType")]
    public enum PhoneType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"MOBILE", Value=0)]
      MOBILE = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"HOME", Value=1)]
      HOME = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WORK", Value=2)]
      WORK = 2
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"reqPerson")]
  public partial class reqPerson : global::ProtoBuf.IExtensible
  {
    public reqPerson() {}
    
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AddressBook")]
  public partial class AddressBook : global::ProtoBuf.IExtensible
  {
    public AddressBook() {}
    
    private readonly global::System.Collections.Generic.List<tutorial.Person> _people = new global::System.Collections.Generic.List<tutorial.Person>();
    [global::ProtoBuf.ProtoMember(1, Name=@"people", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<tutorial.Person> people
    {
      get { return _people; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}