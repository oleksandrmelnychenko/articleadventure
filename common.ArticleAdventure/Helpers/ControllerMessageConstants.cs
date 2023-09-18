using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.Helpers
{
    public class ControllerMessageConstants
    {
        public class TagMessage
        {
            public const string AddTag = "Status add successfully.";
            public const string RemoveTag = "Status remove successfully.";
            public const string UpdateTag = "Status update successfully.";
            public const string AllMainTag = "Get All Main Tags successfully.";
            public const string AllSupTag = "Get All Article successfully.";
            public const string GetTag = "Successfully pulled data.";
        }
        public class UserMessage
        {
            public const string FullUpdateUserProfileAsync = "User successfully updated";
            public const string EmailConformation = "New user successfully updated Email";
            public const string GetNetUidUser = "Get user successfully";
            public const string UpdatePassword = "User successfully updated Password";
            public const string UpdateAccountInformation = "User successfully updated Account Information";
            public const string UpdateEmail = "User successfully updated Email";
            public const string SetFavoriteArticle = "Set favorite Article successfully";
            public const string GetAllFavoriteArticle = "Get All favorite Article successfully";
            public const string GetFavoriteArticle = "Get favorite Article successfully";
            public const string RemoveFavoriteArticle = "Remove favorite Article successfully";
            public const string CreateUserProfileAsync = "New user successfully created";
            
        }
        public class ArticlesMessage
        {
            public const string AddMainArticle = "Status add successfully";
            public const string RemoveArticle = "Status remove successfully";
            public const string UpdateArticle = "Status update successfully";
            public const string GetAllArticle = "Get All MainArticle successfully";
            public const string GetAllFilterDateTimeArticles = "Get All Filtered DateTime MainArticle successfully";
            public const string GetUserAllArticle = "Get All User MainArticle successfully";
            public const string GetUserStripePayments = "Get All StripePayments successfully";
            public const string GetMainArticle = "Get MainArticle successfully";
            public const string GetSupArticle = "Get SupArticle successfully";
            public const string GetUserMainArticle = "Get User MainArticle successfully";
            public const string AllArticleFilterSupTags = "Get All MainArticle filter SupTags successfully";
        }
        public class StripeMessage
        {
            public const string CheckoutOrderBuyNowMainArticle = "Status Checkout Buy MainArticle now successfully";
            public const string CheckoutOrderBuyNowSupArticle = "Status Checkout Buy now SupArticle successfully";
            public const string CheckoutOrderBuyCartMain = "Status Checkout Buy Cart successfully";
            public const string CheckoutSuccessMainArticle = "Checkout success Main Article successfully";
            public const string CheckoutSuccessSupArticle = "Checkout success Sup Article successfully";
            public const string CheckoutSuccessCart = "Checkout success cart successfully";
            public const string CheckoutFailed = "Checkout Failed";
            public const string CheckPaymentsHaveUser = "Checkout Payment successfully";
        }
    }
}
