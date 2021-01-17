using FluentAssertions;
using FluentAssertions.Events;
using FluentValidation.Results;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Tests.Helpers
{
    public static class TestExtensions
    {
        public static void AssertPropertyChanged<T>(this IMonitor<T> monitor, params string[] properties)
        {
            var propertiesChanged = monitor.GetChangedProperties();
            var intersec = propertiesChanged.Intersect(properties);

            intersec.Should().BeEquivalentTo(properties);
        }

        public static void AssertPropertyNotChanged<T>(this IMonitor<T> monitor, params string[] properties)
        {
            var propertiesChanged = monitor.GetChangedProperties();
            var except = propertiesChanged.Except(properties);

            except.Should().BeEquivalentTo(propertiesChanged);
        }

        public static string[] GetChangedProperties<T>(this IMonitor<T> monitor)
        {
            return monitor.OccurredEvents
                .SelectMany(e => e.Parameters)
                .OfType<PropertyChangedEventArgs>()
                .Select(arg => arg.PropertyName)
                .Distinct().ToArray();
        }

        public static void AssertCollectionChanged<T>(this IMonitor<T> monitor, params IEnumerable[] collections)
        {
            var collectionItemTypes = collections.Select(GetGenericType).ToArray();
            var collectionsChanged = monitor.GetChangedCollections();
            var intersec = collectionsChanged.Intersect(collectionItemTypes);

            intersec.Should().BeEquivalentTo(collectionItemTypes);
        }

        public static void AssertCollectionNotChanged<T>(this IMonitor<T> monitor, params IEnumerable[] collections)
        {
            var collectionItemTypes = collections.Select(GetGenericType).ToArray();
            var collectionsChanged = monitor.GetChangedCollections();
            var except = collectionsChanged.Except(collectionItemTypes);

            except.Should().BeEquivalentTo(collectionsChanged);
        }

        public static string[] GetChangedCollections<T>(this IMonitor<T> monitor)
        {
            var parameters = monitor.OccurredEvents.SelectMany(e => e.Parameters).ToArray();
            var collectionParameters = parameters.OfType<NotifyCollectionChangedEventArgs>().ToArray();
            var chengedElements = collectionParameters
                .SelectMany(arg =>
                {
                    var elements = new List<object>();
                    if (arg.OldItems != null) elements.AddRange(arg.OldItems.Cast<object>());
                    if (arg.NewItems != null) elements.AddRange(arg.NewItems.Cast<object>());
                    return elements;
                }).ToArray();
            var changedTypes = chengedElements.Select(e => e.GetType().Name).Distinct().ToArray();
            return changedTypes;
        }

        private static string GetGenericType(IEnumerable collection)
        {
            var type = collection.GetType();
            var generics = type.GetGenericArguments();
            return generics.FirstOrDefault()?.Name;
        }

        public static void AssertErrorsCount(this ValidationResult result, int count)
        {
            result.Should().NotBeNull().And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(count);
        }
    }
}
