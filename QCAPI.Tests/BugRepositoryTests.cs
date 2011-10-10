using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using QCAPI.Model;
using QCAPI.QCModel;
using QCAPI.Repositories;

namespace QCAPI.Tests
{
    [TestFixture]
    public class BugRepositoryTests
    {
        [Test]
        public void when_using_maptobug_expect_qcbug_mapped_to_bug_appropriately()
        {
            // arrange
            var originalBug = new QCBug {bg_Description = "Desc", bg_Status = "Status", bg_Summary =  "Summary"};
            var expectedBug = new Bug {Description = "Desc", Status = "Status", Summary = "Summary"};

            // act 
            var actualBug = new BugRepository().MapToBug().Invoke(originalBug);

            // assert
            Assert.That(actualBug.Description, Is.EqualTo(expectedBug.Description));
            Assert.That(actualBug.Status, Is.EqualTo(expectedBug.Status));
            Assert.That(actualBug.Summary, Is.EqualTo(expectedBug.Summary));
        }

        public void when_parsing_bug_comments_expect_something_sensible()
        {
            // arrange
            const string commentString = @"<html><body><font color=""#000080""><b>James Radford &lt;jamesr&gt;, 16/08/2011: </b></font> will look into this now<br><font color=""#000080""><b>________________________________________</b></font><br><font color=""#000080""><b>James Radford &lt;jamesr&gt;, 16/08/2011: </b></font> this might not be a problem now because at some point before we were displaying the passwords within the text fields as normal characters rather than spots as it should be for all password related fields.  Think this is now resolved for the password confirm at least..<br><br><font color=""#000080""><b>________________________________________</b></font><br><font color=""#000080""><b>James Radford &lt;jamesr&gt;, 16/08/2011: </b></font> this seems okay now, will ask for a demo when you're available.<br><font color=""#000080""><b>________________________________________</b></font><br><font color=""#000080""><b>James Radford &lt;jamesr&gt;, 16/08/2011: </b></font> have spoken to you (Stephen) and apparently you've already tested this after Ian's check in.  Marking as fixed.<br><font color=""#000080""><b>________________________________________</b></font><br><font color=""#000080""><b>Stephen Botchey &lt;stephenb&gt;, 16/08/2011: </b></font> There has been a change in the requirememnt for this field so defect rejected</body></html>";
            const string expectedAuthorValues = "jamesrjamesrjamesrjamesrstephenb";
            const string expectedCommentValue = "will look into this nowthis might not be a problem now because at some point before we were displaying the passwords within the text fields as normal characters rather than spots as it should be for all password related fields. Think this is now resolved for the password confirm at least..this seems okay now, will ask for a demo when youre available.have spoken to you (Stephen) and apparently youve already tested this after Ians check in. Marking as fixed.There has been a change in the requirememnt for this field so defect rejected";

            // act
            var authorResults = BugHelper.ParseComments(commentString).Select(x => x.Author);
            var commentResults = BugHelper.ParseComments(commentString).Select(x => x.Value);

            // assert
            var actualValue = authorResults.Aggregate(string.Empty, (current, item) => current + item);
            Assert.That(actualValue, Is.EqualTo(expectedAuthorValues));

            var actualCommentResults = commentResults.Aggregate(string.Empty, (current, item) => current + item);
            Assert.That(actualCommentResults, Is.EqualTo(expectedCommentValue));
        }

        [Test]
        public void when_finding_word_before_comma_expect_valid_result()
        {
            // arrange
            const string input = "Stephen Botchey stephenb, 16/08/2011: There has been a change in the requirememnt for this field so defect rejected";
            const string expectedResult = "stephenb";

            // act
            var result = BugHelper.WordBeforeComma(input);

            // assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
