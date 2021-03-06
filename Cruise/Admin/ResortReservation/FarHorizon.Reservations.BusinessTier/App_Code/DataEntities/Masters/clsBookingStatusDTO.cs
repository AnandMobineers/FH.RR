using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.App_Code.DataEntities.Masters
{
    public class clsBookingStatusDTO
    {
        private int _iBookingStatusId;
        private string _sBookingStatusType;
        private long _lBookingStatusColor;

        public int BookingStatusId
        {
            get { return _iBookingStatusId; }
            set { _iBookingStatusId = value; }
        }
        
        public string BookingStatusType
        {
            get { return _sBookingStatusType; }
            set { _sBookingStatusType = value; }
        }
        
        public long BookingStatusColor
        {
            get { return _lBookingStatusColor; }
            set { _lBookingStatusColor = value; }
        }

    }
}
