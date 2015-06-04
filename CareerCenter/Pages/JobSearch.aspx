<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobSearch.aspx.cs" Inherits="CareerCenter.Pages.JobSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src ="/js/ga.js"></script>
    <link href="../CareerCenter.css" rel='stylesheet' type='text/css' />
</head>
<body onload="getResults()">
    <form id="form1" runat="server">
        <p id='instructionText'>Enter your search criteria below to browse open jobs.</p>
        <div id="QueryEntry" style="background-color: rgba(225, 225, 200, 0.33);">
            <table class="toolTable">
                <tr>
                    <td class='toolBar keyWords'>
                        <asp:TextBox ID="txtKeywords" class='toolControl' runat="server" placeholder="Job title, industry, skills, key words..."></asp:TextBox>
                    </td>
                    <td class='toolBar postalCode'>
                        <asp:TextBox ID="txtPostalCode" class='toolControl' runat="server" placeholder="Postal Code"></asp:TextBox>
                    </td>
<%--                    <td class='toolBar searchRange'>
                        <select id="ddlRange">
                            <option value="Any">Any Distance</option>
                            <option value="Remote">Remote Only</option>
                            <option value="25">Within 25 Miles</option>
                            <option value="50">Within 50 Miles</option>
                            <option value="100">Within 100 Miles</option>
                        </select>
                    </td>--%>
                    <td class='toolBar searchButton'>
                        <img id="imgSearch" class="imgButton" src="/Images/SearchButton.png" onclick="getResults()" onmouseover="" />
                    </td>

                </tr>
            </table>
        </div>

        <div id="topPageNav" style="display: none; margin-top:5px;">
            <div id="topPrevPage" class="navGuide" onclick="navigatePrevious()">&lt;&lt;Previous Page</div>
            <div id="topNav_1" class="navItem" onclick="navigate($('#Nav_1').text())">1</div>
            <div id="topElipseFront" class="elipse" style="width: 25px;">...</div>
            <div id="topNav_2" class="navItem" onclick="navigate($('#Nav_2').text())">2</div>
            <div id="topNav_3" class="navItem" onclick="navigate($('#Nav_3').text())">3</div>
            <div id="topElipseBack" class="elipse" style="width: 25px;">...</div>
            <div id="topNav_4" class="navItem" onclick="navigate($('#Nav_4').text())">240</div>
            <div id="topNextPage" class="navGuide" onclick="navigateNext()">Next Page&gt;&gt;</div>
            <div style="clear:both;"></div>
        </div>

        <div id="Results">
        </div>
        <div id="noResults" style="display: none;">
            It looks like there are no open jobs to meet your search terms. Try expanding your search, or check back later to search again. Or <a href='UploadResume.aspx'> upload your resume </a> to our talent database, so we can contact you when a job opens that matches your skills.
        </div>
        <div id="PageNav" style="display: none;">
            <hr />
            <div id="PrevPage" class="navGuide" onclick="navigatePrevious()">&lt;&lt;Previous Page</div>
            <div id="Nav_1" class="navItem" onclick="navigate($('#Nav_1').text())">1</div>
            <div id="ElipseFront" class="elipse" style="width: 25px;">...</div>
            <div id="Nav_2" class="navItem" onclick="navigate($('#Nav_2').text())">2</div>
            <div id="Nav_3" class="navItem" onclick="navigate($('#Nav_3').text())">3</div>
            <div id="ElipseBack" class="elipse" style="width: 25px;">...</div>
            <div id="Nav_4" class="navItem" onclick="navigate($('#Nav_4').text())">240</div>
            <div id="NextPage" class="navGuide" onclick="navigateNext()">Next Page&gt;&gt;</div>
            <div></div>
        </div>
        <input id="txtIndex" type="hidden" value="1"/>

        <script type="text/javascript">

            $(document).keypress(function (event) {
                if (event.keyCode == 13 || event.which == 13) //KTC: Some broswers respect which, some browsers respect keyCode.
                {
                    getResults();
                }
            });

            function navigatePrevious() {
                var target = parseInt($('#txtIndex').val());
                navigate(target-1);
            }

            function navigateNext() {
                var target = parseInt($('#txtIndex').val());
                navigate(target+1);
            }

            function navigate(index) {
                var pageItems = 5 //Set the number of items on each page
                var numItems = $('.SummaryWrapper').length;

                if (index < 1 || index > numItems) {
                    //Invalid request.  Do nothing.
                    return;
                }

                if (numItems == 0) {
                    $('#PageNav').hide();
                    $('#topPageNav').hide();
                    return; //Short circuit the whole thing is there is nothing to show
                }
                else {
                    $('#txtIndex').val(index);
                    $('#PageNav').show();
                    $('#topPageNav').show();
                }

                var pages = Math.floor(numItems / pageItems);
                if (numItems % pageItems != 0) { pages++; }

                var firstItem = ((index - 1) * pageItems) + 1;
                var lastItem = (index * pageItems);

                if (lastItem > numItems) {
                    lastItem = numItems;
                }

                if (pages > 4) {
                    //This is to put the nav buttons back if they have been previously wiped.  Downstream code expects them to be shown...
                    for (var loop = 1; loop <= 4; loop++) {
                        $('#Nav_' + loop).show();
                        $('#topNav_' + loop).show();
                    }
                    $('#PrevPage').show();
                    $('#topPrevPage').show();
                    $('#NextPage').show();
                    $('#topNextPage').show();
                }

                if (pages <= 4) {
                    for (var loop = 1; loop <= 4; loop++) {
                        if (loop <= pages) {
                            $('#Nav_' + loop).html(loop);
                            $('#topNav_' + loop).html(loop);
                            if (loop == index) {
                                $('#Nav_' + loop).removeClass().addClass("navItemCurrent");
                                $('#topNav_' + loop).removeClass().addClass("navItemCurrent");
                            }
                            else {
                                $('#Nav_' + loop).removeClass().addClass("navItem");
                                $('#topNav_' + loop).removeClass().addClass("navItem");
                            }
                            $('#Nav_' + loop).show();
                            $('#topNav_' + loop).show();
                        }
                        else {
                            $('#Nav_' + loop).hide();
                            $('#topNav_' + loop).hide();
                        }
                    }
                    $('#ElipseFront').hide();
                    $('#ElipseBack').hide();
                    $('#PrevPage').hide();
                    $('#NextPage').hide();

                    $('#topElipseFront').hide();
                    $('#topElipseBack').hide();
                    $('#topPrevPage').hide();
                    $('#topNextPage').hide();

                }
                else if (index == 1) {
                    $('#PrevPage').removeClass().addClass('navGuide');
                    $('#Nav_1').html('1');
                    $('#Nav_1').removeClass().addClass("navItemCurrent");
                    $('#ElipseFront').hide();
                    $('#Nav_2').html('2');
                    $('#Nav_2').removeClass().addClass("navItem");
                    $('#Nav_3').html('3');
                    $('#Nav_3').removeClass().addClass("navItem");
                    $('#ElipseBack').show();
                    $('#Nav_4').html(pages);
                    $('#Nav_4').removeClass().addClass("navItem");
                    $('#NextPage').removeClass().addClass('navGuideAvailable');

                    $('#topPrevPage').removeClass().addClass('navGuide');
                    $('#topNav_1').html('1');
                    $('#topNav_1').removeClass().addClass("navItemCurrent");
                    $('#topElipseFront').hide();
                    $('#topNav_2').html('2');
                    $('#topNav_2').removeClass().addClass("navItem");
                    $('#topNav_3').html('3');
                    $('#topNav_3').removeClass().addClass("navItem");
                    $('#topElipseBack').show();
                    $('#topNav_4').html(pages);
                    $('#topNav_4').removeClass().addClass("navItem");
                    $('#topNextPage').removeClass().addClass('navGuideAvailable');
                }
                else if (index == pages) {
                    $('#PrevPage').removeClass().addClass('navGuideAvailable');
                    $('#Nav_1').html('1');
                    $('#Nav_1').removeClass().addClass("navItem");
                    $('#ElipseFront').show();
                    $('#Nav_2').html(index - 2);
                    $('#Nav_2').removeClass().addClass("navItem");
                    $('#Nav_3').html(index - 1);
                    $('#Nav_3').removeClass().addClass("navItem");
                    $('#ElipseBack').hide();
                    $('#Nav_4').html(pages);
                    $('#Nav_4').removeClass().addClass("navItemCurrent");
                    $('#NextPage').removeClass().addClass('navGuide');

                    $('#topPrevPage').removeClass().addClass('navGuideAvailable');
                    $('#topNav_1').html('1');
                    $('#topNav_1').removeClass().addClass("navItem");
                    $('#topElipseFront').show();
                    $('#topNav_2').html(index - 2);
                    $('#topNav_2').removeClass().addClass("navItem");
                    $('#topNav_3').html(index - 1);
                    $('#topNav_3').removeClass().addClass("navItem");
                    $('#topElipseBack').hide();
                    $('#topNav_4').html(pages);
                    $('#topNav_4').removeClass().addClass("navItemCurrent");
                    $('#topNextPage').removeClass().addClass('navGuide');
                }
                else {
                    $('#PrevPage').removeClass().addClass('navGuideAvailable');
                    $('#Nav_1').html('1');
                    $('#Nav_1').removeClass().addClass("navItem");

                    $('#topPrevPage').removeClass().addClass('navGuideAvailable');
                    $('#topNav_1').html('1');
                    $('#topNav_1').removeClass().addClass("navItem");

                    if (index != 2) {
                        //This avoids oddiy of having 1 listed twice
                        $('#Nav_2').html(index - 1);
                        $('#Nav_2').removeClass().addClass("navItem");
                        $('#Nav_3').html(index);
                        $('#Nav_3').removeClass().addClass("navItemCurrent");

                        $('#topNav_2').html(index - 1);
                        $('#topNav_2').removeClass().addClass("navItem");
                        $('#topNav_3').html(index);
                        $('#topNav_3').removeClass().addClass("navItemCurrent");

                        if (index != 3) {
                            //This avoids case where elipse appears between 1 and 2, which doesn't make much sense...
                            $('#ElipseFront').show();
                            $('#topElipseFront').show();
                        }
                        else
                        {
                            $('#ElipseFront').show();
                            $('#topElipseFront').show();
                        }
                    }
                    else {
                        $('#Nav_2').html(2);
                        $('#Nav_2').removeClass().addClass("navItemCurrent");
                        $('#Nav_3').html(3);
                        $('#Nav_3').removeClass().addClass("navItem");
                        $('#ElipseFront').hide();

                        $('#topNav_2').html(2);
                        $('#topNav_2').removeClass().addClass("navItemCurrent");
                        $('#topNav_3').html(3);
                        $('#topNav_3').removeClass().addClass("navItem");
                        $('#topElipseFront').hide();
                    }

                    if (index != pages - 1) {
                        //This avoids case where elipse appears between pentultimate and last page
                        $('#ElipseBack').show();
                        $('#topElipseBack').show();
                    }
                    else {
                        $('#ElipseBack').hide();
                        $('#topElipseBack').hide();
                    }
                    $('#Nav_4').html(pages);
                    $('#Nav_4').removeClass().addClass("navItem");
                    $('#NextPage').removeClass().addClass('navGuideAvailable');

                    $('#topNav_4').html(pages);
                    $('#topNav_4').removeClass().addClass("navItem");
                    $('#topNextPage').removeClass().addClass('navGuideAvailable');
                }

                //So much for that.  Now show the items that match the selected page
                $('.SummaryWrapper').hide();
                for (var loop = firstItem; loop <= lastItem; loop++) {
                    $('#SummaryWrapper_' + loop).show();
                }
            }

            function getResults() {
                var keywords = document.getElementById('<%=txtKeywords.ClientID%>').value;
                var postalcode = encodeURIComponent(document.getElementById('<%=txtPostalCode.ClientID%>').value);
                //KTC:06/02/2015 - Remove ability to choose range
                var range = 0; //encodeURIComponent(document.getElementById('ddlRange').value);

                //Last chance verification can happen here before doing the post stuff



                //Here's where we build the post request
                var xmlhttp = new XMLHttpRequest();
                var url = "/Ajax/JobQuery.aspx";
                var postdata = "keywords=" + encodeURIComponent(keywords) + "&postalcode=" + encodeURIComponent(postalcode) + "&range=" + encodeURIComponent(range);

                xmlhttp.open("POST", url, true);

                //Send the proper header information along with the request
                xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
                xmlhttp.setRequestHeader("Content-length", postdata.length);

                xmlhttp.onreadystatechange = function () {
                    if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                        var response = xmlhttp.responseText;
                        showResults(response);
                    }
                }

                xmlhttp.send(postdata);
            }

            function showResults(response) {
                document.getElementById("Results").innerHTML = response;
                if ($('.SummaryWrapper').length < 1) {
                    $('#noResults').show();
                    $('#Results').hide();
                }
                else {
                    $('#noResults').hide();
                    $('#Results').show();
                    navigate(1)
                }
            }

        </script>
    </form>

</body>

</html>
