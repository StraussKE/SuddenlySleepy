using System;
using System.Linq;
using Xunit;
using SuddenlySleepy.Models;
using SuddenlySleepy.Repositories;

namespace SuddenlySleepyTests
{
    public class DonationRepoTests
    {
        [Fact]
        public void TestDirectDonationCreation()
        {
            // Arrange
            var testDonationId = new Guid();
            var testAmount = 500;
            var testDonationDate = DateTime.Now;
                // leaving out test donor because of Identity integration.  Since anonymous donations are permissible, this is a realistic scenario

            // Act
            var testDonation = new Donation
            {
                DonationId = testDonationId,
                DonationAmount = testAmount,
                DonationDate = testDonationDate,
            };

            // Assert
            Assert.NotNull(testDonation); // checks that the donation was created
            Assert.Equal(500, testDonation.DonationAmount);
        }

        [Fact]
        public void TestDirectDonationRepoAddDonation()
        {
            // Arrange
            var testDonation1 = new Donation
            {
                DonationId = new Guid(),
                DonationAmount = 150,
                DonationDate = DateTime.Now,
            };
            var testDonation2 = new Donation
            {
                DonationId = new Guid(),
                DonationAmount = 300,
                DonationDate = DateTime.Now,
            };

            var testDonationRepo = new FakeDonationRepo();

            // Act
            testDonationRepo.AddDonation(testDonation1);
            testDonationRepo.AddDonation(testDonation2);

            // Assert
            Assert.Equal(2, testDonationRepo.Donations.Count());
            Assert.Equal(300, testDonationRepo.Donations.ToList().Last().DonationAmount);
        }

        [Fact]
        public void TestQueryable()
        {
            // Arrange
            var testDonationRepo = new FakeDonationRepo();

            testDonationRepo.AddDonation(
                new Donation
                {
                    DonationId = new Guid(),
                    DonationAmount = 150,
                    DonationDate = DateTime.Now,
                });
            testDonationRepo.AddDonation(
                new Donation
                {
                    DonationId = new Guid(),
                    DonationAmount = 300,
                    DonationDate = DateTime.Now,
                });
            testDonationRepo.AddDonation(
                new Donation
                {
                    DonationId = new Guid(),
                    DonationAmount = 300,
                    DonationDate = DateTime.Now,
                });
            testDonationRepo.AddDonation(
                new Donation
                {
                    DonationId = new Guid(),
                    DonationAmount = 300,
                    DonationDate = DateTime.Now,
                });

            // Act
            var countTestUniqueDonation = testDonationRepo.Donations.Where(e => e.DonationAmount == 150).ToList().Count();
            var countTestLikeDonations = testDonationRepo.Donations.Where(e => e.DonationAmount == 300).ToList().Count();
            var countTestMadeToday = testDonationRepo.Donations.Where(e => e.DonationDate.Date == DateTime.Now.Date).ToList().Count();

            // Assert
            Assert.Equal(1, countTestUniqueDonation);
            Assert.Equal(3, countTestLikeDonations);
            Assert.Equal(4, countTestMadeToday);
        }
    }
}
