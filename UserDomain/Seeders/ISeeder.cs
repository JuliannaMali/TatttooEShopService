using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Seeders;

public interface ISeeder
{
    public Task Seed();
}
