﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data
{
    public interface IWorkEnvironment
    {
        IWorkContextScope CreateWorkContextScope();
    }
}