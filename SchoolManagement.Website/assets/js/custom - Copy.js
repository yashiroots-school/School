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


$(document).ready(function () {
    var expandedWidth = 280;
    var collapsedWidth = 95;

    $('.menu_toggle').on('click', function () {
        var $sidebar = $('.sidebarLeft');
        var $header = $('.header_wrapp');
        var $mainContent = $('.main_content');
        var $menuToggle = $(this);

        if ($sidebar.hasClass('collapsed')) {
            $sidebar.removeClass('collapsed').animate({ width: expandedWidth }, 300);
            $header.animate({ 'padding-left': expandedWidth }, 300);
            $mainContent.animate({ 'padding-left': expandedWidth }, 300); 
            $('.menu_text').fadeIn(200);
        } else {
            $sidebar.addClass('collapsed').animate({ width: collapsedWidth }, 300);
            $header.animate({ 'padding-left': collapsedWidth }, 300); 
            $mainContent.animate({ 'padding-left': collapsedWidth }, 300); 
            $('.menu_text').fadeOut(200);
        }
        $menuToggle.toggleClass('active');
    });
});


$(document).ready(function () {
    var expandedWidth = 320;
    var collapsedWidth = 0;

    $('.filter_icon').on('click', function () {
        var $sidebar = $('.sidebarRight');
        var $header = $('.header_wrapp');
        var $mainContent = $('.main_content');
        var $menuToggle = $(this);

        if ($sidebar.hasClass('collapsed')) {
            $sidebar.removeClass('collapsed').animate({ width: expandedWidth }, 300);
            $header.animate({ 'padding-right': expandedWidth }, 300);
            $mainContent.animate({ 'padding-right': expandedWidth }, 300); 
            $('.menu_text').fadeIn(200);
        } else {
            $sidebar.addClass('collapsed').animate({ width: collapsedWidth }, 300);
            $header.animate({ 'padding-right': collapsedWidth }, 300); 
            $mainContent.animate({ 'padding-right': collapsedWidth }, 300); 
            $('.menu_text').fadeOut(200);
        }
        $menuToggle.toggleClass('active');
    });
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

// School Overview Chart
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
    
    function generateWeekCalendar() {
      calendarDates.innerHTML = "";
    
      // Get the day of the week for the current date (0 = Sunday, 6 = Saturday)
      let currentWeekDay = currentDates.getDay();
    
      // Calculate the start and end of the current week
      let weekStart = new Date(currentYear, currentMonth, currentDay - currentWeekDay);
      let weekEnd = new Date(currentYear, currentMonth, currentDay + (6 - currentWeekDay));
    
      // Display the month and year in the header
      calendarMonth.textContent = `${weekStart.toLocaleString("default", { month: "long" })} ${currentYear}`;
    
      // Generate the dates for the current week
      for (let i = 0; i < 7; i++) {
        const weekDate = new Date(weekStart);
        weekDate.setDate(weekStart.getDate() + i);
    
        const dateDiv = document.createElement("div");
        dateDiv.textContent = weekDate.getDate();
        dateDiv.classList.add("week-date");
    
        // Highlight the current day
        if (weekDate.getDate() === currentDay && weekDate.getMonth() === currentMonth) {
          dateDiv.classList.add("selected");
        }
    
        calendarDates.appendChild(dateDiv);
      }
    }
    
    // Initialize the calendar to show the current week
    generateWeekCalendar();
     



const attendanceCtx = document.getElementById('attendanceChart').getContext('2d');
  new Chart(attendanceCtx, {
    type: 'bar',
    data: {
      labels: ['Class 1', 'Class 2', 'Class 3', 'Class 4', 'Class 5', 'Class 6', 'Class 7', 'Class 8', 'Class 9', 'Class 10', 'Class 11', 'Class 12'],
      datasets: [
        {
          label: 'Attendance',
          data: [125000, 100000, 75000, 100000, 75000, 85000, 95000, 105000, 110000, 120000, 115000, 130000],
          backgroundColor: [
            '#4ce4d5', '#4a90e2', '#f5a623', '#7ed321', '#d0021b',
            '#bd10e0', '#f8e71c', '#8b572a', '#b8e986', '#50e3c2',
            '#417505', '#9013fe',
          ],
          borderWidth: 1,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      scales: {
        y: {
          beginAtZero: true,
          ticks: {
            stepSize: 12500, // Dividing max 150,000 by 12 for 12 tick marks
            callback: function (value) {
              return value.toLocaleString(); // Formats numbers with commas
            },
          },
          max: 150000, // Explicitly set the maximum value for the Y-axis
        },
      },
    },
  });

    // Marks Pie Chart
    const marksCtx = document.getElementById('marksChart').getContext('2d');
    new Chart(marksCtx, {
      type: 'pie',
      data: {
        labels: [
          'Math', 'Science', 'English', 'History', 'Geography',
          'Computer', 'Sports', 'Art', 'Music', 'Other',
        ],
        datasets: [
          {
            label: 'Marks Distribution',
            data: [12, 15, 10, 8, 12, 9, 14, 7, 8, 5],
            backgroundColor: [
              '#f45b69', '#4a90e2', '#50e3c2', '#f8e71c', '#b8e986',
              '#f5a623', '#bd10e0', '#7ed321', '#d0021b', '#9013fe',
            ],
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
      },
    });



 



