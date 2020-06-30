/*
 * This file is automatically generated; any changes will be lost. 
 */

#nullable enable

using Beef.Entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Service.Common.Entities
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PersonArgs : EntityBase, IEquatable<PersonArgs>
    {
        #region Privates

        private string? _firstName = "Hello";
        private string? _lastName = "World";

        #endregion

        #region Properties

        [JsonProperty("firstName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Display(Name="First Name")]
        public string? FirstName
        {
            get => _firstName;
            set => SetValue(ref _firstName, value, false, StringTrim.UseDefault, StringTransform.UseDefault, nameof(FirstName));
        }
        
        [JsonProperty("lastName", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Display(Name="Last Name")]
        public string? LastName
        {
            get => _lastName;
            set => SetValue(ref _lastName, value, false, StringTrim.UseDefault, StringTransform.UseDefault, nameof(LastName));
        }

        #endregion

        #region IsEquitable
        
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is PersonArgs val))
                return false;

            return Equals(val);
        }

        public bool Equals(PersonArgs? value)
        {
            if (((object)value!) == ((object)this))
                return true;
            else if (((object)value!) == null)
                return false;

            return base.Equals((object)value)
                && Equals(FirstName, value.FirstName)
                && Equals(LastName, value.LastName);
        }

        public static bool operator == (PersonArgs? a, PersonArgs? b) => Equals(a, b);

        public static bool operator != (PersonArgs? a, PersonArgs? b) => !Equals(a, b);

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(FirstName);
            hash.Add(LastName);

            return base.GetHashCode() ^ hash.ToHashCode();
        }

        #endregion

        #region ICopyFrom

        public override void CopyFrom(object from)
        {
            var fval = ValidateCopyFromType<PersonArgs>(from);
            CopyFrom(fval);
        }

        public void CopyFrom(PersonArgs from)
        {
            CopyFrom((EntityBase)from);
            FirstName = from.FirstName;
            LastName = from.LastName;
        }

        #endregion

        #region ICloneable

        public override object Clone()
        {
            var clone = new PersonArgs();
            clone.CopyFrom(this);
            return clone;
        }        

        #endregion
        
        #region ICleanUp

        public override void CleanUp()
        {
            base.CleanUp();
            FirstName = Cleaner.Clean(FirstName, StringTrim.UseDefault, StringTransform.UseDefault);
            LastName = Cleaner.Clean(LastName, StringTrim.UseDefault, StringTransform.UseDefault);
        }

        public override bool IsInitial
        {
            get
            {
              return Cleaner.IsInitial(FirstName)
                  && Cleaner.IsInitial(LastName);
            }
        }

        #endregion
    }
}

#nullable restore