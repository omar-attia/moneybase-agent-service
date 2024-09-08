using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agent.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddIndicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Agent_SeniorityId",
                table: "agent",
                newName: "IX_agent_SeniorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_IsOfficeHours",
                table: "team",
                column: "IsOfficeHours");

            migrationBuilder.CreateIndex(
                name: "IX_Team_ShiftEndTime",
                table: "team",
                column: "ShiftEndTime");

            migrationBuilder.CreateIndex(
                name: "IX_Team_ShiftStartTime",
                table: "team",
                column: "ShiftStartTime");

            migrationBuilder.CreateIndex(
                name: "IX_PendingQueuedSession_SessionId",
                table: "pending-queued-session",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgentAssignment_IsCompleted",
                table: "agent-assignment",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_AgentAssignment_SessionId",
                table: "agent-assignment",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agent_MaxConcurrentChats",
                table: "agent",
                column: "MaxConcurrentChats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Team_IsOfficeHours",
                table: "team");

            migrationBuilder.DropIndex(
                name: "IX_Team_ShiftEndTime",
                table: "team");

            migrationBuilder.DropIndex(
                name: "IX_Team_ShiftStartTime",
                table: "team");

            migrationBuilder.DropIndex(
                name: "IX_PendingQueuedSession_SessionId",
                table: "pending-queued-session");

            migrationBuilder.DropIndex(
                name: "IX_AgentAssignment_IsCompleted",
                table: "agent-assignment");

            migrationBuilder.DropIndex(
                name: "IX_AgentAssignment_SessionId",
                table: "agent-assignment");

            migrationBuilder.DropIndex(
                name: "IX_Agent_MaxConcurrentChats",
                table: "agent");

            migrationBuilder.RenameIndex(
                name: "IX_agent_SeniorityId",
                table: "agent",
                newName: "IX_Agent_SeniorityId");
        }
    }
}
