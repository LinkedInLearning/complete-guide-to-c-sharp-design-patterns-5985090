/*
Facade Pattern

Summary:
The Facade Pattern provides a simplified interface to a complex subsystem by hiding the complexities of the subsystem from clients. It defines a higher-level interface that makes the subsystem easier to use by providing a unified interface to a set of interfaces in the subsystem.

Problem to Solve:
In social media management, posting content requires interacting with multiple platform APIs (Facebook, Twitter, Instagram), each with different authentication, formatting, and posting requirements. The challenge is to provide a simple "post to all platforms" interface that hides the complexity of managing multiple social media APIs simultaneously.
*/

using System;

namespace DotNetDesignPatterns.Patterns.Structural
{
    /// <summary>
    /// Simplified interface for social media operations that hides complex subsystem interactions
    /// </summary>
    public interface ISocialMediaService
    {
        void Post(string message);
    }

    public class FacebookService : ISocialMediaService
    {
        public void Post(string message)
        {
            Console.WriteLine($"Posting to Facebook: {message}");
        }
    }

    public class TwitterService : ISocialMediaService
    {
        public void Post(string message)
        {
            Console.WriteLine($"Posting to Twitter: {message}");
        }
    }

    public class InstagramService : ISocialMediaService
    {
        public void Post(string message)
        {
            Console.WriteLine($"Posting to Instagram: {message}");
        }
    }

    public class SocialMediaFacade : ISocialMediaService
    {
        private readonly ISocialMediaService _facebook;
        private readonly ISocialMediaService _twitter;
        private readonly ISocialMediaService _instagram;

        public SocialMediaFacade()
        {
            _facebook = new FacebookService();
            _twitter = new TwitterService();
            _instagram = new InstagramService();
        }

        public void Post(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message cannot be null or empty", nameof(message));

            _facebook.Post(message);
            _twitter.Post(message);
            _instagram.Post(message);
        }
    }

    /// <summary>
    /// Creates a social media facade that simplifies posting to multiple platforms
    /// </summary>
    public static class FacadePattern
    {
        public static ISocialMediaService CreateSocialMediaFacade()
        {
            return new SocialMediaFacade();
        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. Individual platform services (Subsystem classes):
       - FacebookService class with Post(string message) method
       - TwitterService class with Post(string message) method  
       - InstagramService class with Post(string message) method
       - Each service handles platform-specific posting logic

    2. Facade implementation:
       - SocialMediaFacade class implementing ISocialMediaService
       - Constructor creates instances of all platform services
       - Post(message) method calls Post() on all individual platform services
       - Provides single interface to complex multi-platform posting

    3. Factory method implementation:
       - CreateSocialMediaFacade() creates and returns SocialMediaFacade instance
       - Facade should be pre-configured with all supported platforms
       - Client only needs to call one method to post to all platforms

    The tests will verify that:
    - Single Post() call reaches all individual platform services
    - Facade hides complexity of managing multiple social media APIs
    - Client code is simplified through the unified interface
    - Complex subsystem operations are coordinated through facade
    */
}