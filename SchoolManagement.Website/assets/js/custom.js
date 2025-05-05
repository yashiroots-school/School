$(document).ready(function () {
    function toggleSubmenu() {
        $('.menu_item').off('click').on('click', function (event) {
            event.preventDefault();
            var $submenu = $(this).next('.submenu');
            $(this).parent().siblings().find('.submenu').slideUp();
            $(this).parent().siblings().find('.menu_item').removeClass('active');
            $submenu.slideToggle();
            $(this).toggleClass('active');
        });
        $('.submenu').removeAttr('style');
    }
    toggleSubmenu();
});


// $(document).ready(function () {
//     var expandedWidth = 280;
//     var collapsedWidth = 95;

//     $('.menu_toggle').on('click', function () {
//         var $sidebar = $('.sidebarLeft');
//         var $header = $('.header_wrapp');
//         var $mainContent = $('.main_content');
//         var $menuToggle = $(this);

//         if ($sidebar.hasClass('collapsed')) {
//             $sidebar.removeClass('collapsed').animate({ width: expandedWidth }, 300);
//             $header.animate({ 'padding-left': expandedWidth }, 300);
//             $mainContent.animate({ 'padding-left': expandedWidth }, 300); 
//             $('.menu_text').fadeIn(200);
//         } else {
//             $sidebar.addClass('collapsed').animate({ width: collapsedWidth }, 300);
//             $header.animate({ 'padding-left': collapsedWidth }, 300); 
//             $mainContent.animate({ 'padding-left': collapsedWidth }, 300); 
//             $('.menu_text').fadeOut(200);
//         }
//         $menuToggle.toggleClass('active');
//     });
// });


// $(document).ready(function () {
//     var expandedWidth = 270;
//     var collapsedWidth = 0;

//     $('.filter_icon').on('click', function () {
//         var $sidebar = $('.sidebarRight');
//         var $header = $('.header_wrapp');
//         var $mainContent = $('.main_content');
//         var $menuToggle = $(this);

//         if ($sidebar.hasClass('collapsed')) {
//             $sidebar.removeClass('collapsed').animate({ width: expandedWidth }, 300);
//             $header.animate({ 'padding-right': expandedWidth }, 300);
//             $mainContent.animate({ 'padding-right': expandedWidth }, 300); 
//         } else {
//             $sidebar.addClass('collapsed').animate({ width: collapsedWidth }, 300);
//             $header.animate({ 'padding-right': collapsedWidth }, 300); 
//             $mainContent.animate({ 'padding-right': collapsedWidth }, 300); 
//         }
//         $menuToggle.toggleClass('active');
//     });
// });


$(document).ready(function () {
    var expandedWidth = 270;
    var collapsedWidth = 0;

    $('.filter_icon').on('click', function () {
        // Screen width condition check
        if (window.matchMedia("(min-width: 992px)").matches) {
            var $sidebar = $('.sidebarRight');
            var $header = $('.header_wrapp');
            var $mainContent = $('.main_content');
            var $menuToggle = $(this);

            if ($sidebar.hasClass('collapsed')) {
                $sidebar.removeClass('collapsed').animate({ width: expandedWidth }, 300);
                $header.animate({ 'padding-right': expandedWidth }, 300);
                $mainContent.animate({ 'padding-right': expandedWidth }, 300); 
            } else {
                $sidebar.addClass('collapsed').animate({ width: collapsedWidth }, 300);
                $header.animate({ 'padding-right': collapsedWidth }, 300); 
                $mainContent.animate({ 'padding-right': collapsedWidth }, 300); 
            }
            $menuToggle.toggleClass('active');
        }
    });
});




$(document).ready(function () {
    const $searchBox = $('.search_box');
    const $notifiBox = $('.notifi_box');

    // Toggle visibility of search box
    $('.search_icon .header_icon').on('click', function (e) {
      e.stopPropagation(); // Prevent triggering document click
      $searchBox.toggle();
      $notifiBox.hide(); // Hide notification box
    });

    // Toggle visibility of notification box
    $('.notifi_icon .header_icon').on('click', function (e) {
      e.stopPropagation(); // Prevent triggering document click
      $notifiBox.toggle();
      $searchBox.hide(); // Hide search box
    });

    // Hide both boxes on body click
    $(document).on('click', function () {
      $searchBox.hide();
      $notifiBox.hide();
    });

    // Prevent hiding when clicking inside search_box or notifi_box
    $searchBox.on('click', function (e) {
      e.stopPropagation();
    });

    $notifiBox.on('click', function (e) {
      e.stopPropagation();
    });
});


$(document).ready(function () {
    $(".chat_icon").click(function () {
        $(".notification_right").addClass("open"); // Adds the class to show the menu
    });

    $(".btnclose").click(function () {
        $(".notification_right").removeClass("open"); // Removes the class to hide the menu
    });
});


$(document).ready(function () {
    $(".filter_icon").click(function () {
        // Screen width condition check
        if (window.matchMedia("(max-width: 991px)").matches) {
            $(".sidebarRight").addClass("open"); // Adds the class to show the menu
        }
    });

    $(".btnclose").click(function () {
        // Screen width condition check
        if (window.matchMedia("(max-width: 991px)").matches) {
            $(".sidebarRight").removeClass("open"); // Removes the class to hide the menu
        }
    });
});


$(document).ready(function () {
    $(".menu_toggle").click(function () {
        // Screen width condition check
        if (window.matchMedia("(max-width: 767px)").matches) {
            $(".sidebarLeft").addClass("open"); // Adds the class to show the menu
        }
    });

    $(".btnclose").click(function () {
        // Screen width condition check
        if (window.matchMedia("(max-width: 767px)").matches) {
            $(".sidebarLeft").removeClass("open"); // Removes the class to hide the menu
        }
    });
});


$(document).ready(function () {
    $('.notification_tabs ul li').on('click', function () {
      // Remove 'active' class from all tabs
      $('.notification_tabs ul li').removeClass('active');
      
      // Add 'active' class to the clicked tab
      $(this).addClass('active');
      
      // Hide all notification boxes and remove 'active' class
      $('.notification_card .notifibox').removeClass('active');
      
      // Show the corresponding notification box
      if ($(this).hasClass('notifinote')) {
        $('.notification_card .notifi_note').addClass('active');
      } else if ($(this).hasClass('notifialert')) {
        $('.notification_card .notifi_alert').addClass('active');
      } else if ($(this).hasClass('notifichat')) {
        $('.notification_card .notifi_chat').addClass('active');
      }
    });
  });




document.addEventListener('DOMContentLoaded', () => {
  const tabs = document.querySelectorAll('.login_menu');
  const cards = document.querySelectorAll('.account_card');
  const lostPasswordCard = document.querySelector('.lost_password_card');
  const lostPasswordLink = document.querySelector('.lostpassword');

  // Handle tab clicks
  tabs.forEach((tab, index) => {
      tab.addEventListener('click', () => {
          // Reset all tabs and hide all cards
          tabs.forEach(t => t.classList.remove('active'));
          cards.forEach(c => (c.style.display = 'none'));
          lostPasswordCard.style.display = 'none'; // Hide lost password card

          // Activate the clicked tab and show the corresponding card
          tab.classList.add('active');
          cards[index].style.display = 'block';
      });
  });

  // Handle "Lost your password?" click
  //lostPasswordLink.addEventListener('click', () => {
  //    // Hide all tabs and cards
  //    tabs.forEach(t => t.classList.remove('active'));
  //    cards.forEach(c => (c.style.display = 'none'));

  //    // Show the lost password card
  //    lostPasswordCard.style.display = 'block';
  //});
});




const performanceCtx = document.getElementById('performanceChart').getContext('2d');
new Chart(performanceCtx, {
  type: 'line',
  data: {
    labels: ['Week 01', 'Week 02', 'Week 03', 'Week 04', 'Week 05', 'Week 06'],
    datasets: [
      {
        label: 'This Week',
        data: [320, 400, 380, 450, 420, 500],
        borderColor: '#4a90e2',
        backgroundColor: 'rgba(74, 144, 226, 0.2)',
        fill: true,
        tension: 0.4,
      },
      {
        label: 'Last Week',
        data: [480, 360, 400, 380, 480, 430],
        borderColor: '#f45b69',
        backgroundColor: 'rgba(244, 91, 105, 0.2)',
        fill: true,
        tension: 0.4,
      },
    ],
  },
  options: {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        labels: {
          font: { size: 14 },
        },
      },
    },
  },
});

//// School Overview Chart
const overviewCtx = document.getElementById('overviewChart').getContext('2d');
const overviewChart = new Chart(overviewCtx, {
  type: 'bar',
  data: {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [
      {
        label: 'Number of Projects',
        data: [50, 60, 80, 100, 70, 60, 80, 90, 70, 80, 90, 100],
        backgroundColor: '#4a90e2',
      },
      {
        label: 'Revenue',
        data: [60, 70, 90, 110, 80, 70, 100, 120, 90, 100, 110, 130],
        borderColor: '#4caf50',
        backgroundColor: 'rgba(76, 175, 80, 0.2)',
        type: 'line',
        fill: true,
        tension: 0.4,
      },
      {
        label: 'Active Projects',
        data: [40, 50, 60, 80, 50, 40, 70, 100, 60, 80, 100, 120],
        borderColor: '#f45b69',
        backgroundColor: 'rgba(244, 91, 105, 0.2)',
        type: 'line',
        fill: false,
        tension: 0.4,
      },
    ],
  },
  options: {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        labels: {
          font: { size: 14 },
        },
      },
    },
  },
});

// Update Overview Chart
function updateOverviewChart(range) {
  const tabs = document.querySelectorAll('.tab');
  tabs.forEach(tab => tab.classList.remove('active'));
  document.querySelector(`.tab[onclick="updateOverviewChart('${range}')"]`).classList.add('active');
  
  // Simulating different data for different ranges
  if (range === 'week') {
    overviewChart.data.datasets[0].data = [10, 15, 20, 25, 30, 35, 40];
  } else if (range === 'month') {
    overviewChart.data.datasets[0].data = [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160];
  } else if (range === 'year') {
    overviewChart.data.datasets[0].data = [200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420];
  } else {
    overviewChart.data.datasets[0].data = [50, 60, 80, 100, 70, 60, 80, 90, 70, 80, 90, 100];
  }
  overviewChart.update();
}


const calendarGrid = document.getElementById('calendarGrid');
    const currentMonthDisplay = document.getElementById('currentMonth');
    const prevMonthBtn = document.getElementById('prevMonth');
    const nextMonthBtn = document.getElementById('nextMonth');

    const today = new Date();
    let currentDate = new Date(today.getFullYear(), today.getMonth(), 1);

    const renderCalendar = () => {
      calendarGrid.innerHTML = '';
      const year = currentDate.getFullYear();
      const month = currentDate.getMonth();

      currentMonthDisplay.textContent = currentDate.toLocaleDateString('en-US', {
        month: 'long',
        year: 'numeric',
      });

      // Days of the week headers
      const daysOfWeek = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
      daysOfWeek.forEach((day) => {
        const header = document.createElement('div');
        header.classList.add('header');
        header.textContent = day;
        calendarGrid.appendChild(header);
      });

      // Get first day of the month and number of days
      const firstDay = new Date(year, month, 1).getDay();
      const daysInMonth = new Date(year, month + 1, 0).getDate();
      const prevDaysInMonth = new Date(year, month, 0).getDate();

      // Previous month's days
      for (let i = firstDay - 1; i >= 0; i--) {
        const day = document.createElement('div');
        day.classList.add('disabled');
        day.textContent = prevDaysInMonth - i;
        calendarGrid.appendChild(day);
      }

      // Current month's days
      for (let i = 1; i <= daysInMonth; i++) {
        const day = document.createElement('div');
        day.classList.add('day');
        day.textContent = i;

        // Highlight today's date
        if (
          year === today.getFullYear() &&
          month === today.getMonth() &&
          i === today.getDate()
        ) {
          day.classList.add('selected');
        }

        day.addEventListener('click', () => {
          document.querySelectorAll('.day').forEach((d) => d.classList.remove('selected'));
          day.classList.add('selected');
        });

        calendarGrid.appendChild(day);
      }

      // Next month's days
      const remainingCells = 42 - calendarGrid.children.length;
      for (let i = 1; i <= remainingCells; i++) {
        const day = document.createElement('div');
        day.classList.add('disabled');
        day.textContent = i;
        calendarGrid.appendChild(day);
      }

      // Disable navigation if out of range (optional)
      prevMonthBtn.disabled = year === today.getFullYear() - 1 && month === 0;
      nextMonthBtn.disabled = year === today.getFullYear() + 1 && month === 11;
    };

    const changeMonth = (direction) => {
      currentDate.setMonth(currentDate.getMonth() + direction);
      renderCalendar();
    };

    prevMonthBtn.addEventListener('click', () => changeMonth(-1));
    nextMonthBtn.addEventListener('click', () => changeMonth(1));

    renderCalendar();



    const calendarDates = document.getElementById("calendar_dates");
    const calendarMonth = document.getElementById("calendar_month");
    
    let currentDates = new Date(); // Get current date
    let currentYear = currentDates.getFullYear();
    let currentMonth = currentDates.getMonth();
    let currentDay = currentDates.getDate();
    
    //function generateWeekCalendar() {
    //  calendarDates.innerHTML = "";
    
    //  // Get the day of the week for the current date (0 = Sunday, 6 = Saturday)
    //  let currentWeekDay = currentDates.getDay();
    
    //  // Calculate the start and end of the current week
    //  let weekStart = new Date(currentYear, currentMonth, currentDay - currentWeekDay);
    //  let weekEnd = new Date(currentYear, currentMonth, currentDay + (6 - currentWeekDay));
    
    //  // Display the month and year in the header
    //  calendarMonth.textContent = `${weekStart.toLocaleString("default", { month: "long" })} ${currentYear}`;
    
    //  // Generate the dates for the current week
    //  for (let i = 0; i < 7; i++) {
    //    const weekDate = new Date(weekStart);
    //    weekDate.setDate(weekStart.getDate() + i);
    
    //    const dateDiv = document.createElement("div");
    //    dateDiv.textContent = weekDate.getDate();
    //    dateDiv.classList.add("week-date");
    
    //    // Highlight the current day
    //    if (weekDate.getDate() === currentDay && weekDate.getMonth() === currentMonth) {
    //      dateDiv.classList.add("selected");
    //    }
    
    //    calendarDates.appendChild(dateDiv);
    //  }
    //}
    
    // Initialize the calendar to show the current week
   /* generateWeekCalendar();*/
     



    



 



