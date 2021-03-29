using System;
using System.Collections.Generic;
using System.Text;

namespace GetContactAPI.Models
{
    public class Meta
    {
        public string requestId { get; set; }
        public int httpStatusCode { get; set; }
    }

    public class TrustScore
    {
        public object score { get; set; }
        public bool showLanding { get; set; }
        public bool getScore { get; set; }
        public int limit { get; set; }
        public int remainingCount { get; set; }
        public bool showOffer { get; set; }
        public bool isColorRed { get; set; }
        public bool showPackages { get; set; }
        public Localizations localizations { get; set; }
    }

    public class Profile
    {
        public object name { get; set; }
        public object surname { get; set; }
        public string phoneNumber { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string displayName { get; set; }
        public int tagCount { get; set; }
        public object profileImage { get; set; }
        public object email { get; set; }
        public TrustScore trustScore { get; set; }
    }

    public class Config
    {
        public object endpoint { get; set; }
    }

    public class SpamInfo
    {
        public object type { get; set; }
        public string degree { get; set; }
    }

    public class RatingOptions
    {
        public bool callHistoryShow { get; set; }
        public bool searchHistoryShow { get; set; }
        public bool spamTabShow { get; set; }
        public bool blockerClose { get; set; }
        public bool showTagsBlockerShow { get; set; }
        public bool showTagsBlockerClose { get; set; }
        public bool showTagsManualShow { get; set; }
        public bool showTagsManualClose { get; set; }
        public bool searchDetailShow { get; set; }
        public bool searchDetailClose { get; set; }
        public bool newsFeedShow { get; set; }
        public bool dialerShow { get; set; }
    }

    public class Provider
    {
        public string mediation { get; set; }
        public string unitId { get; set; }
        public bool status { get; set; }
        public bool video { get; set; }
    }

    public class Locations
    {
        public bool showTagsResult { get; set; }
        public bool showTagsResultDetail { get; set; }
        public bool manuelSearch { get; set; }
        public bool shareSearch { get; set; }
        public bool showTagsShare { get; set; }
    }

    public class Interstitial
    {
        public int interval { get; set; }
        public List<Provider> providers { get; set; }
        public Locations locations { get; set; }
        public int noFillInterval { get; set; }
        public int searchCount { get; set; }
        public int reducedInterval { get; set; }
    }

    public class SearchHistory
    {
        public string mediation { get; set; }
        public string unitId { get; set; }
        public bool status { get; set; }
        public bool video { get; set; }
    }

    public class CallHistory
    {
        public string mediation { get; set; }
        public string unitId { get; set; }
        public bool status { get; set; }
        public bool video { get; set; }
    }

    public class SearchDetail
    {
        public string mediation { get; set; }
        public string unitId { get; set; }
        public bool status { get; set; }
        public bool video { get; set; }
    }

    public class NotificationList
    {
        public string mediation { get; set; }
        public string unitId { get; set; }
        public bool status { get; set; }
        public bool video { get; set; }
    }

    public class NewsFeed
    {
        public string mediation { get; set; }
        public string unitId { get; set; }
        public bool status { get; set; }
        public bool video { get; set; }
    }

    public class Native
    {
        public List<SearchHistory> searchHistory { get; set; }
        public List<CallHistory> callHistory { get; set; }
        public List<SearchDetail> searchDetail { get; set; }
        public object callResult { get; set; }
        public List<NotificationList> notificationList { get; set; }
        public List<NewsFeed> newsFeed { get; set; }
    }

    public class AdSettings
    {
        public Interstitial interstitial { get; set; }
        public Native native { get; set; }
    }

    public class Localizations
    {
        public string title { get; set; }
        public string description { get; set; }
        public string btnTitle { get; set; }
    }

    public class Search
    {
        public int limit { get; set; }
        public int remainingCount { get; set; }
        public bool showOffer { get; set; }
        public bool isColorRed { get; set; }
        public bool showPackages { get; set; }
        public Localizations localizations { get; set; }
    }

    public class NumberDetail
    {
        public int limit { get; set; }
        public int remainingCount { get; set; }
        public bool showOffer { get; set; }
        public bool isColorRed { get; set; }
        public bool showPackages { get; set; }
        public Localizations localizations { get; set; }
    }

    public class Usage
    {
        public Search search { get; set; }
        public NumberDetail numberDetail { get; set; }
        public TrustScore trustScore { get; set; }
    }

    public class SubscriptionInfo
    {
        public bool isPro { get; set; }
        public bool isTrialUsed { get; set; }
        public object storeProductId { get; set; }
        public Usage usage { get; set; }
        public string premiumType { get; set; }
        public string premiumTypeName { get; set; }
        public bool showTrustScoreUsage { get; set; }
        public bool showSubscriptionInfo { get; set; }
        public bool showSubscriptionPackages { get; set; }
        public bool isMainSubscriptionMenuActive { get; set; }
        public bool showStatics { get; set; }
        public bool showWhoLookedMyProfile { get; set; }
        public string renewDate { get; set; }
        public string receiptStartDate { get; set; }
        public string receiptEndDate { get; set; }
        public object lastPackageText { get; set; }
        public string subsInfoButtonText { get; set; }
        public string subsInfoButtonIntroText { get; set; }
    }

    public class Bot
    {
        public string name { get; set; }
        public string url { get; set; }
        public bool status { get; set; }
        public bool showBotPreview { get; set; }
    }

    public class Result
    {
        public Profile profile { get; set; }
        public string badge { get; set; }
        public bool closeAdBtn { get; set; }
        public int newTagCount { get; set; }
        public int deletedTagCount { get; set; }
        public string deletedTagRequestButton { get; set; }
        public Config config { get; set; }
        public SpamInfo spamInfo { get; set; }
        public object numberFix { get; set; }
        public bool searchedHimself { get; set; }
        public bool shouldInvite { get; set; }
        public bool limitedResult { get; set; }
        public RatingOptions ratingOptions { get; set; }
        public string premiumType { get; set; }
        public bool showPrivatePopup { get; set; }
        public bool dialerPermission { get; set; }
        public AdSettings adSettings { get; set; }
        public object inviteText { get; set; }
        public object inviteTextUI { get; set; }
        public SubscriptionInfo subscriptionInfo { get; set; }
        public List<object> customEvents { get; set; }
        public object premiumDialog { get; set; }
        public bool privateMode { get; set; }
        public List<Bot> bots { get; set; }
    }

    public class Root
    {
        public Meta meta { get; set; }
        public Result result { get; set; }
    }


}
