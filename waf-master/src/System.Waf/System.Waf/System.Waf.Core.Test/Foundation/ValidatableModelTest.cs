﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Waf.Foundation;
using System.Waf.UnitTesting;

namespace Test.Waf.Foundation
{
    [TestClass]
    public class ValidatableModelTest
    {
        [TestMethod]
        public void HasAndGetErrorsWithPropertyValidation()
        {
            Person person = new Person();

            // Person is invalid but until now nobody has validated this object.

            Assert.IsFalse(person.HasErrors);            
            Assert.IsFalse(person.GetErrors().Any());

            // Validate person and see that Name is required.

            AssertHelper.PropertyChangedEvent(person, x => x.HasErrors, () 
                => AssertErrorsChangedEvent(person, () => person.Validate()));
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(Person.NameRequiredError, person.GetErrors().Single().ErrorMessage);
            Assert.AreEqual(Person.NameRequiredError, person.GetErrors(nameof(Person.Name)).Single().ErrorMessage);

            // Set a valid name.

            AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = "Bill");
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.GetErrors().Any());
            Assert.IsFalse(((INotifyDataErrorInfo)person).GetErrors(nameof(Person.Name)).Cast<object>().Any());

            // Set another valid name; ErrorsChanged event must not be called.

            AssertHelper.ExpectedException<AssertFailedException>(() => AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = "Steve"));

            // Set the same name again; ErrorsChanged event must not be called.

            AssertHelper.ExpectedException<AssertFailedException>(() => AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = "Steve"));

            // Call validate on the valid Person; ErrorsChanged event must not be called.

            AssertHelper.ExpectedException<AssertFailedException>(() => AssertErrorsChangedEvent(person, () => person.Validate()));

            // Set an invalid name (null)

            AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = null);
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(Person.NameRequiredError, person.GetErrors().Single().ErrorMessage);
            Assert.AreEqual(Person.NameRequiredError, person.GetErrors(nameof(Person.Name)).Single().ErrorMessage);

            // Set an invalid email address that creates two additional validation errors.

            person.Email = "TooLongAndAnInvalidEmailAddress@";
            bool isValid = true;
            AssertErrorsChangedEvent(person, () => isValid = person.Validate());
            Assert.IsFalse(isValid);
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(3, person.GetErrors().Count());
            Assert.AreEqual(2, person.GetErrors("Email").Count());
            Assert.IsTrue(person.GetErrors("Email").Any(x => x.ErrorMessage == Person.EmailInvalidError));
            Assert.IsTrue(person.GetErrors("Email").Any(x => x.ErrorMessage == Person.EmailLengthError));
            
            // Set a valid name and email address

            AssertErrorsChangedEvent(person, x => x.Name, () => person.Name = "Bill");
            person.Email = "h.p@h.edu";
            AssertErrorsChangedEvent(person, () => isValid = person.Validate());
            Assert.IsTrue(isValid);
            Assert.IsFalse(person.HasErrors);
            Assert.IsFalse(person.GetErrors().Any());
            Assert.IsFalse(person.GetErrors("Email").Any());
        }

        [TestMethod]
        public void HasAndGetErrorsWithObjectValidation()
        {
            Person person = new Person() { Name = "Bill" };
            person.Validate();
            Assert.IsFalse(person.HasErrors);

            // Create an entity error

            var entityError = new ValidationResult("My entity error");
            person.EntityError = entityError;

            bool isValid = true;
            AssertHelper.PropertyChangedEvent(person, x => x.HasErrors, () =>
                AssertErrorsChangedEvent(person, () => isValid = person.Validate()));
            Assert.IsFalse(isValid);
            Assert.IsTrue(person.HasErrors);
            Assert.AreEqual(entityError, person.GetErrors().Single());
            Assert.AreEqual(entityError, person.GetErrors("").Single());
            Assert.AreEqual(entityError, person.GetErrors(null).Single());
        }

        [TestMethod]
        public void ValidatePropertyTest()
        {
            Person person = new Person();

            string name = null;
            AssertHelper.ExpectedException<ArgumentException>(() => person.SetPropertyAndValidate(ref name, "Bill", null));
            AssertHelper.ExpectedException<ArgumentException>(() => person.ValidateProperty("Bill", null));

            Assert.IsTrue(person.SetPropertyAndValidate(ref name, "Bill", nameof(Person.Name))); // Value has been changed
            Assert.IsFalse(person.SetPropertyAndValidate(ref name, "Bill", nameof(Person.Name))); // Value has not been changed

            person.Name = "Bill";
            Assert.IsTrue(person.ValidateProperty(person.Name, nameof(Person.Name))); // Name is valid
            person.Name = null;
            Assert.IsFalse(person.ValidateProperty(person.Name, nameof(Person.Name))); // Name is invalid
        }
        
        [TestMethod]
        public void SerializationWithDcsTest()
        {
            var serializer = new DataContractSerializer(typeof(Person));

            using (MemoryStream stream = new MemoryStream())
            {
                Person person = new Person() { Name = "Hugo" };
                serializer.WriteObject(stream, person);

                stream.Position = 0;
                Person newPerson = (Person)serializer.ReadObject(stream);
                Assert.AreEqual(person.Name, newPerson.Name);
            }
        }

        private static void AssertErrorsChangedEvent<T>(T model, Action raiseErrorsChanged) where T : INotifyDataErrorInfo
        {
            int errorsChangedCount = 0;

            EventHandler<DataErrorsChangedEventArgs> handler = (sender, e) =>
            {
                Assert.AreEqual(model, sender);
                if (string.IsNullOrEmpty(e.PropertyName))
                {
                    errorsChangedCount++;
                }
            };

            model.ErrorsChanged += handler;
            raiseErrorsChanged();
            model.ErrorsChanged -= handler;

            Assert.AreEqual(1, errorsChangedCount);
        }
        
        private static void AssertErrorsChangedEvent<T>(T model, Expression<Func<T, object>> expression, Action raiseErrorsChanged) where T : INotifyDataErrorInfo
        {
            string propertyName = AssertHelper.GetProperty(expression).Name;
            int errorsChangedCount = 0;

            EventHandler<DataErrorsChangedEventArgs> handler = (sender, e) =>
            {
                Assert.AreEqual(model, sender);
                if (e.PropertyName == propertyName)
                {
                    errorsChangedCount++;
                }
            };

            model.ErrorsChanged += handler;
            raiseErrorsChanged();
            model.ErrorsChanged -= handler;

            Assert.AreEqual(1, errorsChangedCount);
        }


        [DataContract]
        private class Person : ValidatableModel, IValidatableObject
        {
            public const string NameRequiredError = "The Name field is required.";
            public const string EmailLengthError = "The field Email must be a string with a maximum length of 10.";
            public const string EmailInvalidError = "The Email field is not a valid e-mail address.";

            private ValidationResult entityError;
            [DataMember]
            private string name;
            [DataMember]
            private string email;

            
            public ValidationResult EntityError 
            {
                get { return entityError; }
                set { entityError = value; }
            }
            
            [Required(ErrorMessage = NameRequiredError)]
            public string Name
            {
                get { return name; }
                set { SetPropertyAndValidate(ref name, value); }
            }

            [EmailAddress(ErrorMessage = EmailInvalidError)]
            [StringLength(10, ErrorMessage = EmailLengthError)]
            public string Email
            {
                get { return email; }
                set { SetProperty(ref email, value); }
            }

            public new bool ValidateProperty(object value, string propertyName)
            {
                return base.ValidateProperty(value, propertyName);
            }

            public new bool SetPropertyAndValidate<T>(ref T field, T value, string propertyName)
            {
                return base.SetPropertyAndValidate(ref field, value, propertyName);
            }


            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var validationResults = new List<ValidationResult>();
                if (EntityError != null) { validationResults.Add(EntityError); }
                return validationResults;
            }
        }
    }
}
