using System;
using Moq;
using NUnit.Framework;
using Puffin.Core.Ecs;
using Puffin.Core.Ecs.Systems;
using Puffin.Core.Events;
using Puffin.Core.IO;

namespace Puffin.Core.UnitTests.Ecs
{
    [TestFixture]
    public class MouseSystemTests
    {
        
        [TestCase(-10, 3)]
        [TestCase(31, 3)]
        [TestCase(100, -5)]

        [TestCase(17, 17)]
        [TestCase(64, 22)]

        [TestCase(-9, 93)]
        [TestCase(28, 53)]
        [TestCase(66, 61)]
        public void OnClickCallbackDoesNotFireIfClickIsOutOfBounds(int clickedX, int clickedY)
        {
            // Arrange
            var mouseProvider = new Mock<IMouseProvider>();

            var eventBus = new EventBus();
            var callbackFired = false;
            var entity = new Entity().Move(20, 10).Mouse((x, y) => callbackFired = true, 32, 32);
            var system = new MouseSystem(eventBus, mouseProvider.Object);
            system.OnAddEntity(entity);
            mouseProvider.Setup(m => m.MouseCoordinates).Returns(new Tuple<int, int>(clickedX, clickedY));

            // Act
            eventBus.Broadcast(EventBusSignal.MouseClicked, null);

            // Assert
            Assert.That(callbackFired, Is.False);
        }

        [Test]
        public void OnClickCallbackFiresIfClickIsInBounds()
        {
            // Arrange
            var mouseProvider = new Mock<IMouseProvider>();
            const int clickedX = 90;
            const int clickedY = 91;

            var eventBus = new EventBus();
            var callbackFired = false;
            var entity = new Entity().Move(77, 88).Mouse((x, y) => 
            {
                Assert.That(x, Is.EqualTo(clickedX));
                Assert.That(y, Is.EqualTo(clickedY));
                callbackFired = true;
            }, 32, 32);
            mouseProvider.Setup(m => m.MouseCoordinates).Returns(new Tuple<int, int>(clickedX, clickedY));
            var system = new MouseSystem(eventBus, mouseProvider.Object);
            system.OnAddEntity(entity);

            // Act
            eventBus.Broadcast(EventBusSignal.MouseClicked, null);
            
            // Assert
            Assert.That(callbackFired, Is.True);
        }

        [Test]
        public void RemoveEntityRemovesEntity()
        {
            // Arrange
            var mouseProvider = new Mock<IMouseProvider>();

            var eventBus = new EventBus();
            var callbackFired = false;
            var entity = new Entity().Move(77, 88).Mouse((x, y) => callbackFired = true, 32, 32);
            mouseProvider.Setup(m => m.MouseCoordinates).Returns(new Tuple<int, int>(90, 90));
            var system = new MouseSystem(eventBus, mouseProvider.Object);
            system.OnAddEntity(entity);

            // Act
            system.OnRemoveEntity(entity);
            eventBus.Broadcast(EventBusSignal.MouseClicked, null);
            
            // Assert: removed entities don't trigger callbacks.
            Assert.That(callbackFired, Is.False);
        }
    }
}