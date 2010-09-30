using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using Marcidia;
using Marcidia.Game.Admin.Repositories;
using System.IO;
using Marcidia.Game.Admin.Services;

namespace Marcidia.Game.Admin
{
    [MarcidiaComponent("Admin Interface", true)]
    public class AdminComponent : MarcidiaComponent
    {
        readonly string DataDirectoryPath;

        public AdminComponent(Mud mud)
            : base(mud)
        {
            DataDirectoryPath = Path.Combine("Data", "Admin");

            IStringHasher stringHasher = new StringHasher("Test", "Test");
            IAdminUserRepository adminUserRepository = new FlatFileAdminUserRepository(DataDirectoryPath);
            IAdminUserService adminUserService = new AdminUserService(stringHasher, adminUserRepository);

            Mud.Services.AddService<IAdminUserService>(adminUserService);
        }

        public override void Initialize()
        {            
        }
    }
}
