using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.View.Model.Secretary;
using WpfApp1.ViewModel.Secretary;

namespace WpfApp1.View.Converter
{
    internal class MeetingConverter : AbstractConverter
    {
        public static MeetingView ConvertMeetingToMeetingView(Meeting meeting, Room room)
        => new MeetingView
        {
            Beginning = meeting.Beginning,
            Nametag = room.Nametag
        };
    }
}
